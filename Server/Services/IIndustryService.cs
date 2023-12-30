using OrganizationApi.Entity;
using OrganizationApi.Services.Base;

namespace OrganizationApi.Services;

public interface IIndustryService : IBaseCrud<Industry>
{
    Task<Industry?> FindOneById(int id);
    Task<Industry?> FindOneByName(string name);
}