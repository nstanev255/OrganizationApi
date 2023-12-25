using Newtonsoft.Json;

namespace OrganizationApi.Dto;

public class OrganizationRequestModel
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public string? Name { get; set; }
    
    [JsonProperty("number_of_employees")]
    public int NumberOfEmployees { get; set; }
    
    [JsonProperty("founded")]
    public string? Founded { get; set; }
    
    [JsonProperty("industry")]
    public IndustryRequestModel? Industry { get; set; }
    
    [JsonProperty("country")]
    public CountryRequestModel? Country { get; set; }
    
}