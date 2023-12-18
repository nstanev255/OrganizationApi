using Microsoft.AspNetCore.Mvc;

namespace OrganizationApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrganizationController : ControllerBase
{
    [HttpGet]
    [Route("hello-world")]
    public Object HelloWorld()
    {
        return new { hello = "world" };
    }
}