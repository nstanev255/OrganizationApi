using OrganizationApi.Dto;

namespace OrganizationApi.Services;

public interface IImportService
{
    public Task<OrganizationImportResponse> ImportOrganizations(List<OrganizationRequestModel> organizations);
}