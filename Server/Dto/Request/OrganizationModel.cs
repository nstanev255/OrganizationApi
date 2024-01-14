using Newtonsoft.Json;
using OrganizationApi.Entity;

namespace OrganizationApi.Dto;

public class OrganizationModel
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

    public OrganizationModel()
    {
    }

    public OrganizationModel(Organization organization)
    {
        Name = organization.Name;
        Description = organization.Description;
        if (organization.Country != null)
        {
            Country = organization.Country.Name;
        }
        if (organization.Industry != null)
        {
            Industry = organization.Industry.Name;
        }

        Founded = organization.Founded.ToString();
        Index = organization.Id;

        NumberOfEmployees = organization.NumberOfEmployees;
        OrganizationId = organization.OrganizationId;
        Website = organization.Website;
    }
}