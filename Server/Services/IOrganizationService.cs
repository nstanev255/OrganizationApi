using OrganizationApi.Entity;
using OrganizationApi.Services.Base;

namespace OrganizationApi.Services;

public interface IOrganizationService : IBaseCrud<Organization>
{
    Task<Organization?> FindOneByOrganizationId(string id);
}