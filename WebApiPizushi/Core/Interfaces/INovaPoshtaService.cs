using Core.Models.NovaPoshta.City;
using Core.Models.NovaPoshta.Department;

namespace Core.Interfaces;

public interface INovaPoshtaService
{
    Task<List<CityItemResponse>> FetchCitiesAsync();
    Task<List<DepartmentItemResponse>> FetchDepartmentsAsync();
}
