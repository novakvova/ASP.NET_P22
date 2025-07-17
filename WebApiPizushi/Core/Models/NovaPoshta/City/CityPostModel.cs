using Newtonsoft.Json;

namespace Core.Models.NovaPoshta.City;

public class CityPostModel
{
    [JsonProperty("apiKey")]
    public string ApiKey { get; set; } = string.Empty;
    [JsonProperty("modelName")]
    public string ModelName { get; set; } = "Address";
    [JsonProperty("calledMethod")]
    public string CalledMethod { get; set; } = "getCities";
    [JsonProperty("methodProperties")]
    public MethodCityProperties? MethodProperties { get; set; }
}

public class MethodCityProperties
{
    public string AreaRef { get; set; } = String.Empty;
}
