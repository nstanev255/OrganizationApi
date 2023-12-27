using Newtonsoft.Json;

namespace OrganizationApi.Dto;

public class OrganizationRequestModel
{
    [JsonProperty("index")]
    public int Index { get; set; }
    
    [JsonProperty("organization_id")]
    public string OrganizationId { get; set; }
    
    [JsonProperty("name")]
    public string? Name { get; set; }
    
    [JsonProperty("website")]
    public string Website { get; set; }
    
    [JsonProperty("country")] 
    public string Country { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("founded")]
    public string? Founded { get; set; }
    
    [JsonProperty("industry")] 
    public string Industry { get; set; }
    
    [JsonProperty("number_of_employees")]
    public int NumberOfEmployees { get; set; }
}