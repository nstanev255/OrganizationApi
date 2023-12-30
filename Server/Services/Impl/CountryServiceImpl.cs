using Microsoft.EntityFrameworkCore;
using Nager.Country;
using OrganizationApi.Context;
using OrganizationApi.Dto;
using OrganizationApi.Entity;
using OrganizationApi.Services.Base;

namespace OrganizationApi.Services.Impl;

public class CountryServiceImpl : BaseCrud<Country>, ICountryService
{
    private readonly ICountryProvider _countryProvider;

    public CountryServiceImpl(ApplicationDbContext context) : base(context.Countries)
    {
        _countryProvider = new CountryProvider();
    }


    public async Task<Country?> FindOneByName(string name)
    {
        return await dao.FirstOrDefaultAsync(c => c.Name == name);
    }

    public string GetCountryCode(string countryName)
    {
        string countryCode = "";
        try
        {
            countryCode = _countryProvider.GetCountryByName(countryName).Alpha2Code.ToString();
        }
        catch
        {
            // Do nothing, we don't care really.
        }

        return countryCode;
    }

    /**
     * We will override the BaseCrud create method, so we can add the country code.
     */
    public async Task<Country> Create(Country entity)
    {
        return await base.Create(new Country
        {
            Name = entity.Name,
            Code = GetCountryCode(entity.Name)
        });
    }

    public async Task<Country> CreateOrThrow(CountryModel entity)
    {
        var exists = await FindOneByName(entity.Name);
        if (exists != null)
        {
            throw new Exception("Country already exists.");
        }

        return await Create(new Country {Name = entity.Name});
    }

    public async Task<Country?> FindOneByIdOrName(int id, string name)
    {
        return await dao.Where(c => c.Id == id || c.Name == name).FirstOrDefaultAsync();
    }

    public async Task<Country> UpdateOrThrow(int id, CountryModel model)
    {
        // If we have a matching entry for either name or id, we must not allow the update to happen.
        // As we can't have two countries with either the same names or the same ids.
        var dbEntity = await FindOneByIdOrName(id, model.Name);
        if (dbEntity == null)
        {
            throw new Exception("Country does not exist");
        }

        if (dbEntity.Name != model.Name)
        {
            var newCode = GetCountryCode(model.Name);
            dbEntity.Name = model.Name;
            dbEntity.Code = newCode;
        }

        return await Update(dbEntity);
    }
}