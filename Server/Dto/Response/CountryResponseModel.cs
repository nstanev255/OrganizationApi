using Newtonsoft.Json;

namespace OrganizationApi.Dto;

public class CountryResponseModel
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("country_code")]
    public string? Code { get; set; }
}