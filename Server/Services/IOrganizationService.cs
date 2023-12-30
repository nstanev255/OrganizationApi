using OrganizationApi.Dto;
using OrganizationApi.Entity;
using OrganizationApi.Services.Base;

namespace OrganizationApi.Services;

public interface IOrganizationService : IBaseCrud<Organization>
{
    Task<Organization?> FindOneByOrganizationId(string id);

    public Task<OrganizationRequestModel> UpdateOrganization(string id, OrganizationUpdateRequestModel model);
    
    public Task<OrganizationImportResponse> ImportOrganizations(List<OrganizationRequestModel> organizations);
    public Task<ImportOrganizationModel> ImportOrganization(OrganizationRequestModel organization);

}