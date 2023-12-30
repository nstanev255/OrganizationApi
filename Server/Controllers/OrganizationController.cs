using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationApi.Dto;
using OrganizationApi.Dto.Jwt;
using OrganizationApi.Services;

namespace OrganizationApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrganizationController : ControllerBase
{

    private readonly IOrganizationService _organizationService;

    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
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
    public async Task<OrganizationImportResponse> ImportOrganizations(List<OrganizationRequestModel> organizationRequest)
    {
        return await _organizationService.ImportOrganizations(organizationRequest);
    }

    [HttpPost]
    [Authorize]
    [Route("")]
    public async Task<ImportOrganizationModel> Create(OrganizationRequestModel model)
    {
        return await _organizationService.ImportOrganization(model);
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    public async Task<OrganizationRequestModel> Update(string id, OrganizationUpdateRequestModel model)
    {
        return await _organizationService.UpdateOrganization(id, model);
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

        // Delete the organization.
        await _organizationService.Delete(organization.Id);

        return Ok(200);
    }
}