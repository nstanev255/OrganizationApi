using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationApi.Dto;
using OrganizationApi.Dto.Jwt;
using OrganizationApi.Services;

namespace OrganizationApi.Controllers;

public class IndustryController : ControllerBase
{
    private readonly IIndustryService _industryService;
    public IndustryController(IIndustryService industryService)
    {
        _industryService = industryService;
    }

    [HttpGet]
    [Authorize]
    [Route("{id}")]
    public async Task<IndustryResponseModel> Read(int id)
    {
        var entity = await _industryService.Read(id.ToString());
        
        // TODO: Create a better error-handling to include http response codes as well.
        if (entity == null)
        {
            throw new Exception("Industry does not exist");
        }

        return new IndustryResponseModel
        {
            Name = entity.Name,
            Id = entity.Id,
        };
    }

    [HttpPost]
    [Authorize]
    [Route("")]
    public async Task<IndustryResponseModel> Create(IndustryModel model)
    {
        var entity = await _industryService.CreateOrThrow(model);

        return new IndustryResponseModel
        {
            Name = entity.Name,
            Id = entity.Id
        };
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    public async Task<IndustryResponseModel> Update(int id, IndustryModel model)
    {
        var entity = await _industryService.UpdateOrThrow(id, model);

        return new IndustryResponseModel
        {
            Name = entity.Name,
            Id = entity.Id
        };
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