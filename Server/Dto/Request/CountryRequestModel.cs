using Newtonsoft.Json;

namespace OrganizationApi.Dto;

public class CountryRequestModel
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("country_code")]
    public string? CountryCode { get; set; }
    
    [JsonProperty("country_name")]
    public string? CountryName { get; set; }
    
}