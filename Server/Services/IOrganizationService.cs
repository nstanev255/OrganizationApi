using OrganizationApi.Dto;
using OrganizationApi.Entity;
using OrganizationApi.Services.Base;

namespace OrganizationApi.Services;

public interface IOrganizationService : IBaseCrud<Organization>
{
    public Task<List<Organization>> GetBiggestOrganizations();
    Task<Organization?> FindOneByOrganizationId(string id);
    public Task<Organization> UpdateOrThrow(string id, OrganizationUpdateRequestModel model);
    public Task<OrganizationImportResponse> ImportOrganizations(List<OrganizationModel> organizations);
    public Task<ImportOrganizationModel> Create(OrganizationModel organization);
    public Task<PdfDocument> GeneratePdfReport(string id);
}