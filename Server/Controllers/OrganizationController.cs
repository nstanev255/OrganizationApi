using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrganizationApi.Dto;
using OrganizationApi.Dto.Jwt;
using OrganizationApi.Services;

namespace OrganizationApi.Controllers;

[ApiController]
[Route("[controller]")]
[DisableRequestSizeLimit]
public class OrganizationController : ControllerBase
{

    private readonly IOrganizationService _organizationService;

    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpGet]
    [Route("")]
    public List<OrganizationModel> ReadAll(int page = 1, int pageSize = 10)
    {
        var organizations = _organizationService.ReadAll(page, pageSize);
        
        if (organizations.IsNullOrEmpty())
        {
            throw new Exception("No records found");
        }

        return new List<OrganizationModel>(organizations.Select(o => new OrganizationModel(o)));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<OrganizationModel> Read(string id)
    {
        var organization = await _organizationService.Read(id);
        if (organization == null)
        {
            throw new Exception("Not found.");
        }

        return new OrganizationModel(organization);
    }

    /**
     * Import organizations.
     *
     * This endpoint is used for bulk importing organizations.
     *
     * Only admin users can bulk import.
     */
    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    [Route("bulk-import")]
    public async Task<OrganizationImportResponse> ImportOrganizations(List<OrganizationModel> organizationRequest)
    {
        return await _organizationService.ImportOrganizations(organizationRequest);
    }

    [HttpGet]
    [Authorize]
    [Route("biggest")]
    public async Task<List<OrganizationModel>> GetBiggestOrganizations()
    {
        var organizations =  await _organizationService.GetBiggestOrganizations();

        if (organizations.IsNullOrEmpty())
        {
            throw new Exception("No records found...");
        }

        return new List<OrganizationModel>(organizations.Select(o => new OrganizationModel(o)));
    }

    [HttpPost]
    [Authorize]
    [Route("")]
    public async Task<ImportOrganizationModel> Create(OrganizationModel model)
    {
        return await _organizationService.Create(model);
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    public async Task<OrganizationModel> Update(string id, OrganizationUpdateRequestModel model)
    {
        var organization = await _organizationService.UpdateOrThrow(id, model);

        return new OrganizationModel(organization);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoles.Admin)]
    [Route("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var organization = await _organizationService.FindOneByOrganizationId(id);
        if (organization == null)
        {
            throw new Exception("Organization does not exist");
        }

        try
        {
            await _organizationService.Delete(organization.Id);
            // Delete the organization.
        }
        catch
        {
            throw new Exception("Could not delete Organization");
        }
        

        return Ok(200);
    }
    
    
    [Authorize]
    [HttpGet]
    [Route("{id}/pdf-report")]
    public async Task<IActionResult> PdfReport(string id)
    {
        var pdfFile = await _organizationService.GeneratePdfReport(id);

        return new FileStreamResult(pdfFile.Stream, "application/pdf")
        {
            FileDownloadName = $"{id}_report"
        };
    }
    
}