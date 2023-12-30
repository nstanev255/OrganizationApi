using OrganizationApi.Entity;

namespace OrganizationApi.Dto;

public class ImportOrganizationModel
{
    public Organization ImportedOrganization { get; set; }
    
    // If we have imported a new country.
    public bool ImportedCountry { get; set; }
    // If we have imported a new industry.
    public bool ImportedIndustry { get; set; }
}