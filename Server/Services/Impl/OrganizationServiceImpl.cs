using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;
using LinqToDB.EntityFrameworkCore;
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
    private readonly IFileService _fileService;

    private readonly ApplicationDbContext _context;

    public OrganizationServiceImpl(
        ApplicationDbContext context,
        IIndustryService industryService,
        ICountryService countryService,
        IFileService fileService) : base(context.Organizations)
    {
        _context = context;
        _industryService = industryService;
        _countryService = countryService;
        _fileService = fileService;
    }


    public async Task<List<Organization>> GetBiggestOrganizations()
    {
        if (!dao.Any())
        {
            throw new Exception("No records found..");
        }

        return await dao.OrderByDescending(x => x.NumberOfEmployees)
            .Take(5)
            .ToListAsync();

        throw new NotImplementedException();
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
            // If the industry is null, we will throw an error that it does not exist.
            if (industry == null)
            {
                throw new Exception("The industry does not exist.");
            }

            if (organization.Industry == null || industry.Name != organization.Industry.Name)
            {
                industry.Organizations.RemoveAll(o => o.OrganizationId == organization.OrganizationId);

                industry.Organizations.Add(organization);
                organization.Industry = industry;

                await _industryService.Update(industry);
            }
        }
        
        if (model.Country != null)
        {
            var country = await _countryService.FindOneByName(model.Country);
            if (country == null)
            {
                throw new Exception("The country does not exist.");
            }

            if (organization.Country == null || country.Name != organization.Country.Name)
            {
                country.Organizations.RemoveAll(o => o.OrganizationId == organization.OrganizationId);
                    
                organization.Country = country;
                country.Organizations.Add(organization);
                    
                await _countryService.Update(country);
            }
        }

        return await Update(organization);
    }

    public async Task<ImportOrganizationModel> Create(OrganizationModel organization)
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

    public Organization? CreateOrganization(
        OrganizationModel organization,
        ImmutableSortedDictionary<string, int> countries,
        ImmutableSortedDictionary<string, int> industries)
    {
        var industry = industries[organization.Industry];
        var country = countries[organization.Country];

        // Validate the website url.
        if (!UrlUtils.ValidateUrlWithUri(organization.Website))
        {
            throw new Exception("Website url is not a valid website url.");
        }


        // We have created or got the industry and the country - now we just need to create the Organization.
        return new Organization
        {
            Name = organization.Name,
            CountryId = country,
            IndustryId = industry,
            Description = organization.Description,
            Id = organization.Index,
            Founded = int.Parse(organization.Founded),
            Website = organization.Website,
            NumberOfEmployees = organization.NumberOfEmployees,
            OrganizationId = organization.OrganizationId
        };
    }

    public async Task<OrganizationImportResponse> ImportOrganizations(List<OrganizationModel> organizations)
    {
        if (dao.Any())
        {
            throw new Exception("Organizations are already imported...");
        }

        var sw = new Stopwatch();
        sw.Start();
        Console.WriteLine("Started Import...");
        var bulk = new List<Organization>(); 
        var countries = GetCountries(organizations);
        var industries = GetIndustries(organizations);
        
        Console.WriteLine("Import countries " + countries.Count);
        Console.WriteLine("Import industries " + industries.Count);
        
        
        // // Pre BULK (PGSQL COPY) create countries and industries.
        await _context.BulkCopyAsync(countries);
        await _context.BulkCopyAsync(industries);

        int importedIndustries = industries.Count;
        int importedCountries = countries.Count;

        var cacheCountries = _countryService.GetAllSortedDict();
        var cacheIndustries = _industryService.GetAllSortedDict();

        Console.WriteLine("BULK copy organizations and industries done...");

        Console.WriteLine("organizations count" + organizations.Count);
        
        foreach (var organization in organizations)
        {
            try
            {
                var dbOrg = CreateOrganization(organization, cacheCountries, cacheIndustries);
                if (dbOrg == null) continue;
                bulk.Add(dbOrg);
            }
            catch
            {
                // we don't care if we get errors, we will just skip the import for the said organization.
            }
        }

        Console.WriteLine("Started bulk copy organizations... " + bulk.Count);

        // PGSQL specific BULK operation.
        await _context.BulkCopyAsync(bulk);
        await _context.SaveChangesAsync();
        
        int importedOrganizations = bulk.Count;
        
        Console.WriteLine("Bulk copy Organizations done..");
        sw.Stop();

        Console.WriteLine("Time taken (seconds) : " + sw.Elapsed.Seconds);
        return new OrganizationImportResponse
        {
            ImportedIndustries = importedIndustries,
            ImportedOrganizations = importedOrganizations,
            ImportedCountries = importedCountries
        };
    }

    private IList<Country> GetCountries(List<OrganizationModel> organizations)
    {
        var countries = new SortedDictionary<string, Country>();

        foreach (var organization in organizations)
        {
            var country = countries.ContainsKey(organization.Country);
            if (!country)
            {
                countries.Add(organization.Country, new Country
                {
                    Name = organization.Country
                });
            }
        }

        return countries.Values.ToList();
    }

    private IList<Industry> GetIndustries(List<OrganizationModel> organizations)
    {
        var industries = new SortedDictionary<string, Industry>();

        foreach (var organization in organizations)
        {
            var industry = industries.ContainsKey(organization.Industry);
           if(!industry)
               industries.Add(organization.Industry, new Industry()
            {
                Name = organization.Industry
            });
        }

        return industries.Values.ToList();
    }

    /**
     * We will override the organization Read function, as we want to read it for a different id.
     */
    public override Task<Organization?> Read(string id)
    {
        return dao.Where(o => o.OrganizationId == id).FirstOrDefaultAsync();
    }

    public async Task<PdfDocument> GeneratePdfReport(string id)
    {
        var organization = await FindOneByOrganizationId(id);

        if (organization == null)
        {
            throw new Exception("Could not find organization.");
        }

        return _fileService.CreatePdfForOrganization(organization);
    }
}