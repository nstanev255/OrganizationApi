using Newtonsoft.Json;
using OrganizationApi.Entity;

namespace OrganizationApi.Dto;

public class CountryResponseModel
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("country_code")]
    public string? Code { get; set; }

    public CountryResponseModel()
    {
    }

    public CountryResponseModel(Country country)
    {
        this.Id = country.Id;
        this.Code = country.Code;
        this.Name = country.Name;
    }
}