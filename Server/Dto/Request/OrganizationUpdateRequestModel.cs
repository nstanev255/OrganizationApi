using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OrganizationApi.Dto;

public class OrganizationUpdateRequestModel
{
    [JsonProperty("name")]
    public string? Name { get; set; }
    
    [JsonProperty("website")]
    public string? Website { get; set; }
    
    [JsonProperty("country")] 
    public string? Country { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("founded")]
    [RegularExpression(@"^(19|20)\d{2}$", ErrorMessage = "The entered year must be between 1900 - 2099")]
    public string? Founded { get; set; }
    
    [JsonProperty("industry")] 
    public string? Industry { get; set; }
    
    [JsonProperty("number_of_employees")]
    public int? NumberOfEmployees { get; set; }
}