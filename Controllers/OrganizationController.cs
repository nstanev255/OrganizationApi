using Microsoft.AspNetCore.Mvc;
using OrganizationApi.Dto;
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
     */
    [HttpPost]
    [Route("import-organizations")]
    public OrganizationImportResponse ImportOrganizations(List<OrganizationRequestModel> organizationRequest)
    {
        return _importService.ImportOrganizations(organizationRequest);
    }


    [HttpGet]
    [Route("hello-world")]
    public Object HelloWorld()
    {
        return new { hello = "world" };
    }
}