using System.Collections.Immutable;
using OrganizationApi.Dto;
using OrganizationApi.Entity;
using OrganizationApi.Services.Base;

namespace OrganizationApi.Services;

public interface ICountryService : IBaseCrud<Country>
{
    ImmutableSortedDictionary<string, int> GetAllSortedDict();
    Task<Country?> FindOneByName(string name);
    Task<Country?> FindOneById(int id, string name);
    string GetCountryCode(string countryName);
    public Task<Country> UpdateOrThrow(int id, CountryModel model);
    public Task<Country> CreateOrThrow(CountryModel model);


}