using OrganizationApi.Entity;

namespace OrganizationApi.Services;

public interface IFileService
{
    PdfDocument CreatePdfForOrganization(Organization organization);
    PdfDocument CreatePdfFromHtml(string html);
}