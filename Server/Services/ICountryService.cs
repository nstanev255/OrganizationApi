using OrganizationApi.Entity;
using OrganizationApi.Services.Base;

namespace OrganizationApi.Services;

public interface ICountryService : IBaseCrud<Country>
{
    Task<Country?> FindOneByName(string name);
    string GetCountryCode(string countryName);

}