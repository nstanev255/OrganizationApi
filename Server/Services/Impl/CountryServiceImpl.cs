using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Nager.Country;
using OrganizationApi.Context;
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
    public Task<Country?> Create(Country entity)
    {
        return base.Create(new Country
        {
            Name = entity.Name,
            Code = GetCountryCode(entity.Name)
        });
    }
}