using Newtonsoft.Json;
using OrganizationApi.Entity;

namespace OrganizationApi.Dto;

public class IndustryResponseModel
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }

    public IndustryResponseModel()
    {
    }

    public IndustryResponseModel(Industry industry)
    {
        Id = industry.Id;
        Name = industry.Name;
    }
}