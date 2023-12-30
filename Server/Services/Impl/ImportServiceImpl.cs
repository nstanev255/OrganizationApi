using OrganizationApi.Dto;
using OrganizationApi.Entity;

namespace OrganizationApi.Services.Impl;

public class ImportServiceImpl : IImportService
{
    private readonly IIndustryService _industryService;
    private readonly ICountryService _countryService;
    private readonly IOrganizationService _organizationService;
    public ImportServiceImpl(
        IIndustryService industryService, 
        ICountryService countryService,
        IOrganizationService organizationService)
    {
        _industryService = industryService;
        _countryService = countryService;
        _organizationService = organizationService;
    }

    public async Task<OrganizationImportResponse> ImportOrganizations(List<OrganizationRequestModel> organizations)
    {
        int importedOrganizations = 0;
        int importedCountries = 0;
        int importedIndustries = 0;
        foreach (var organization in organizations)
        {
            // If we already have this organization, then we will just skip it...
            var dbOrganization = await _organizationService.FindOneByOrganizationId(organization.OrganizationId);
            if (dbOrganization != null)
            {
                continue;
            }

            var industry = await _industryService.FindOneByName(organization.Industry);
            if (industry == null)
            {
                industry = await _industryService.Create(
                    new Industry { Name = organization.Industry });
                ++importedIndustries;
            }

            var country = await _countryService.FindOneByName(organization.Country);
            if (country == null)
            {
                country = await _countryService.Create(new Country { Name = organization.Country });
                ++importedCountries;
            }
            
            // We have created or got the industry and the country - now we just need to create the Organization.
            dbOrganization = new Organization
            {
                Name = organization.Name,
                Country = country,
                Industry = industry,
                Description = organization.Description,
                Id = organization.Index,
                Founded = int.Parse(organization.Founded),
                Website = organization.Website,
                NumberOfEmployees = organization.NumberOfEmployees,
                OrganizationId = organization.OrganizationId
            };
            
            // Create the organization.
            await _organizationService.Create(dbOrganization);
            
            // Update the country and industry relationships.
            country.Organizations.Add(dbOrganization);
            industry.Organizations.Add(dbOrganization);
            
            // Commit them to the database.
            await _countryService.Update(country);
            await _industryService.Update(industry);

            ++importedOrganizations;
        }

        return new OrganizationImportResponse
        {
            ImportedIndustries = importedIndustries,
            ImportedOrganizations = importedOrganizations,
            ImportedCountries = importedCountries
        };
    }
}