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

    private readonly IImportService _importService;

    public OrganizationController(IImportService importService)
    {
        _importService = importService;
    }

    /**
     * Import organizations.
     *
     * This endpoint is used for bulk importing organizations.
     *
     * Only admin users can import.
     */
    [HttpPost]
    [Authorize]
    [Route("bulk-import")]
    public async Task<OrganizationImportResponse> ImportOrganizations(List<OrganizationRequestModel> organizationRequest)
    {
        return await _importService.ImportOrganizations(organizationRequest);
    }


    [HttpGet]
    [Route("hello-world")]
    public Object HelloWorld()
    {
        return new { hello = "world" };
    }
}