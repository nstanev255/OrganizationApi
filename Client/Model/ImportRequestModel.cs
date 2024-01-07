using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;

namespace Client.Model;

public class ImportRequestModel
{
    [JsonProperty("index")]
    [Index(0)]
    public string Index { get; set; }
    
    [JsonProperty("organization_id")]
    [Index(1)]
    public string OrganizationId { get; set; }
    
    [JsonProperty("name")]
    [Index(2)]
    public string Name { get; set; }
    
    [JsonProperty("website")]
    [Index(3)]
    public string Website { get; set; }
    
    [JsonProperty("country")]
    [Index(4)]
    public string Country { get; set; }

    [JsonProperty("founded")]
    [Index(5)]
    public string Founded { get; set; }
    
    [JsonProperty("description")]
    [Index(6)]
    public string Description { get; set; }
    
    [JsonProperty("industry")]
    [Index(7)]
    public string Industry { get; set; }
    
    [JsonProperty("number_of_employees")]
    [Index(8)]
    public string NumberOfEmployees { get; set; }
}