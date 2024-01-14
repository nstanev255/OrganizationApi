using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrganizationApi.Dto;
using OrganizationApi.Dto.Jwt;
using OrganizationApi.Services;

namespace OrganizationApi.Controllers;

[ApiController]
[Route("[controller]")]
public class IndustryController : ControllerBase
{
    private readonly IIndustryService _industryService;
    public IndustryController(IIndustryService industryService)
    {
        _industryService = industryService;
    }

    [HttpGet]
    [Route("")]
    public List<IndustryResponseModel> ReadAll(int page = 1, int pageSize = 10)
    {
        var industries = _industryService.ReadAll(page, pageSize);
        if (industries.IsNullOrEmpty())
        {
            throw new Exception("No records for industry");
        }

        return new List<IndustryResponseModel>(industries.Select(i => new IndustryResponseModel(i)));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IndustryResponseModel> Read(int id)
    {
        var entity = await _industryService.Read(id.ToString());
        
        // TODO: Create a better error-handling to include http response codes as well.
        if (entity == null)
        {
            throw new Exception("Industry does not exist");
        }

        return new IndustryResponseModel(entity);
    }

    [HttpPost]
    [Authorize]
    [Route("")]
    public async Task<IndustryResponseModel> Create(IndustryModel model)
    {
        var entity = await _industryService.CreateOrThrow(model);
        return new IndustryResponseModel(entity);
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    public async Task<IndustryResponseModel> Update(int id, IndustryModel model)
    {
        var entity = await _industryService.UpdateOrThrow(id, model);

        return new IndustryResponseModel(entity);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoles.Admin)]
    [Route("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _industryService.Delete(id);
            return Ok();
        }
        catch
        {
            throw new Exception("Could not delete industry.");
        }

    }
}