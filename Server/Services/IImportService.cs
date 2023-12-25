using OrganizationApi.Dto;

namespace OrganizationApi.Services;

public interface IImportService
{
    public OrganizationImportResponse ImportOrganizations(List<OrganizationRequestModel> organizations);
}