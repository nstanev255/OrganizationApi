using Newtonsoft.Json;

namespace OrganizationApi.Dto;

public class IndustryRequestModel
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public string? Name { get; set; }
    
}