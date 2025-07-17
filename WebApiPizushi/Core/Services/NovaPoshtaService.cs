using AutoMapper;
using Core.Interfaces;
using Core.Models.NovaPoshta.Area;
using Core.Models.NovaPoshta.City;
using Core.Models.NovaPoshta.Department;
using Domain;
using Domain.Entities.Delivery;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Core.Services;

public class NovaPoshtaService(IMapper mapper, IHttpClientFactory httpClientFactory, AppDbPizushiContext context) : INovaPoshtaService
{
    private readonly HttpClient http = httpClientFactory.CreateClient();
    private readonly string url = "https://api.novaposhta.ua/v2.0/json/";
    private readonly string apiKey = "27e98316d8976226c4d4185fa2650f10";

    private async Task<List<AreaItemResponse>> FetchAreasAsync()
    {
        var request = new AreaPostModel
        {
            ApiKey = apiKey,
            ModelName = "Address",
            CalledMethod = "getAreas",
            MethodProperties = new MethodProperties()
        };

        var result = await SendRequestAsync<AreaResponse>(request);

        return result?.Success == true && result.Data != null
            ? result.Data
            : new List<AreaItemResponse>();
    }

    public async Task<List<CityItemResponse>> FetchCitiesAsync()
    {
        var areas = await FetchAreasAsync();
        var cities = new List<CityItemResponse>();

        foreach (var area in areas)
        {
            var modelRequest = new CityPostModel
            {
                ApiKey = apiKey,
                MethodProperties = new MethodCityProperties { AreaRef = area.Ref }
            };

            var result = await SendRequestAsync<CityResponse>(modelRequest);
            if (result?.Data != null && result.Success)
            {
                foreach (var city in result.Data)
                {
                    var entity = mapper.Map<CityEntity>(city);
                    await context.Cities.AddAsync(entity);

                    Console.WriteLine($"Add city: {city.Description}");
                }

                cities.AddRange(result.Data);
            }
        }

        await context.SaveChangesAsync();
        return cities;
    }

    public async Task<List<DepartmentItemResponse>> FetchDepartmentsAsync()
    {
        var departments = new List<DepartmentItemResponse>();
        var cities = await FetchCitiesAsync();

        foreach (var city in cities)
        {
            var cityEntity = await context.Cities.FirstOrDefaultAsync(x => x.Name == city.Description);
            if (cityEntity == null)
                continue;

            var modelRequest = new DepartmentPostModel
            {
                ApiKey = apiKey,
                MethodProperties = new MethodDepatmentProperties { CityRef = city.Ref }
            };

            var result = await SendRequestAsync<DepartmentResponse>(modelRequest);
            if (result?.Data != null && result.Success)
            {
                foreach (var department in result.Data)
                {
                    var entity = mapper.Map<PostDepartmentEntity>(department);
                    entity.CityId = cityEntity.Id;

                    await context.PostDepartments.AddAsync(entity);
                    Console.WriteLine($"Add post department: {department.Description}");
                }

                departments.AddRange(result.Data);
            }
        }

        await context.SaveChangesAsync();
        return departments;
    }

    private async Task<T?> SendRequestAsync<T>(object modelRequest) where T : class
    {
        string json = JsonConvert.SerializeObject(modelRequest, new JsonSerializerSettings
        {
            ContractResolver = (modelRequest is AreaPostModel)
                ? new CamelCasePropertyNamesContractResolver()
                : null,
            Formatting = Formatting.Indented
        });

        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await http.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResp = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonResp);
        }

        return null;
    }
}
