using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OrganizationApi.Dto;

public class IndustryModel
{
    [JsonProperty("name")]
    public string Name { get; set; }
}