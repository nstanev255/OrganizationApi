using Microsoft.EntityFrameworkCore;
using OrganizationApi.Context;
using OrganizationApi.Dto;
using OrganizationApi.Entity;
using OrganizationApi.Services.Base;
using OrganizationApi.Utils;

namespace OrganizationApi.Services.Impl;

public class OrganizationServiceImpl : BaseCrud<Organization>, IOrganizationService
{
    private readonly IIndustryService _industryService;
    private readonly ICountryService _countryService;

    public OrganizationServiceImpl(
        ApplicationDbContext context,
        IIndustryService industryService,
        ICountryService countryService) : base(context.Organizations)
    {
        _industryService = industryService;
        _countryService = countryService;
    }


    public async Task<Organization?> FindOneByOrganizationId(string id)
    {
        return await dao.FirstOrDefaultAsync(o => o.OrganizationId == id);
    }

    public async Task<Organization> UpdateOrThrow(string id, OrganizationUpdateRequestModel model)
    {
        var organization = await FindOneByOrganizationId(id);
        if (organization == null)
        {
            throw new Exception("Organization does not exist.");
        }

        if (model.Name != null && model.Name != organization.Name)
        {
            organization.Name = model.Name;
        }

        if (model.Description != null && model.Description != organization.Description)
        {
            organization.Description = model.Description;
        }

        if (model.NumberOfEmployees.HasValue && model.NumberOfEmployees != organization.NumberOfEmployees)
        {
            organization.NumberOfEmployees = model.NumberOfEmployees.Value;
        }

        if (model.Founded != null && int.Parse(model.Founded) != organization.Founded)
        {
            organization.Founded = int.Parse(model.Founded);
        }

        if (model.Website != null && model.Website != organization.Website)
        {
            if (!UrlUtils.ValidateUrlWithUri(model.Website))
                throw new Exception("The website url that is given is not valid.");

            organization.Website = model.Website;
        }

        if (model.Industry != null)
        {
            var industry = await _industryService.FindOneByName(model.Industry);
            // If the industry is null, we will create it from here.
            if (industry == null)
            {
                industry = await _industryService.Create(new Industry { Name = model.Industry });
            }

            if (industry.Name != organization.Industry.Name)
            {
                industry.Organizations.RemoveAll(o => o.OrganizationId == organization.OrganizationId);

                industry.Organizations.Add(organization);
                organization.Industry = industry;

                await _industryService.Update(industry);
            }

            if (model.Country != null)
            {
                var country = await _countryService.FindOneByName(model.Country);
                if (country == null)
                {
                    country = await _countryService.Create(new Country { Name = model.Country });
                }

                if (country.Name != organization.Country.Name)
                {
                    country.Organizations.RemoveAll(o => o.OrganizationId == organization.OrganizationId);
                    
                    organization.Country = country;
                    country.Organizations.Add(organization);
                    
                    await _countryService.Update(country);
                }
            }
        }

        return await Update(organization);
    }

    public async Task<ImportOrganizationModel> ImportOrganization(OrganizationRequestModel organization)
    {
        // If we already have this organization, then we will just skip it...
        var dbOrganization = await FindOneByOrganizationId(organization.OrganizationId);
        if (dbOrganization != null)
        {
            throw new Exception("The organization already exists.");
        }

        var importedOrganizationModel = new ImportOrganizationModel();

        var industry = await _industryService.FindOneByName(organization.Industry);
        if (industry == null)
        {
            industry = await _industryService.Create(
                new Industry { Name = organization.Industry });
            importedOrganizationModel.ImportedIndustry = true;
        }

        var country = await _countryService.FindOneByName(organization.Country);
        if (country == null)
        {
            country = await _countryService.Create(new Country { Name = organization.Country });
            importedOrganizationModel.ImportedCountry = true;
        }

        // Validate the website url.
        if (!UrlUtils.ValidateUrlWithUri(organization.Website))
        {
            throw new Exception("Website url is not a valid website url.");
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
        await Create(dbOrganization);

        // Update the country and industry relationships.
        country.Organizations.Add(dbOrganization);
        industry.Organizations.Add(dbOrganization);

        // Commit them to the database.
        await _countryService.Update(country);
        await _industryService.Update(industry);

        importedOrganizationModel.ImportedOrganization = dbOrganization;
        return importedOrganizationModel;
    }

    public async Task<OrganizationImportResponse> ImportOrganizations(List<OrganizationRequestModel> organizations)
    {
        int importedOrganizations = 0;
        int importedCountries = 0;
        int importedIndustries = 0;
        foreach (var organization in organizations)
        {
            try
            {
                var newOrg = await ImportOrganization(organization);

                if (newOrg.ImportedCountry)
                    ++importedCountries;

                if (newOrg.ImportedIndustry)
                    ++importedIndustries;

                ++importedOrganizations;
            }
            catch
            {
                // we don't care if we get errors, we will just skip the import for the said organization.
            }
        }

        return new OrganizationImportResponse
        {
            ImportedIndustries = importedIndustries,
            ImportedOrganizations = importedOrganizations,
            ImportedCountries = importedCountries
        };
    }

    /**
     * We will override the organization Read function, as we want to read it for a different id.
     */
    public override Task<Organization?> Read(string id)
    {
        return dao.Where(o => o.OrganizationId == id).FirstOrDefaultAsync();
    }
}