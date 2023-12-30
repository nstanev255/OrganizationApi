using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationApi.Dto;
using OrganizationApi.Dto.Jwt;
using OrganizationApi.Entity;
using OrganizationApi.Services;

namespace OrganizationApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CountryController : ControllerBase
{
    private readonly ICountryService _countryService;
    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpPost]
    [Authorize]
    [Route("")]
    public async Task<CountryResponseModel> Create(CountryModel model)
    {
        var entity = await _countryService.Create(new Country { Name = model.Name });
        return new CountryResponseModel()
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name
        };
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    public async Task<CountryResponseModel> Update(int id, CountryModel model)
    {
        var entity = await _countryService.UpdateOrThrow(id, model);

        return new CountryResponseModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code
        };
    }

    [HttpDelete]
    [Authorize(Roles = UserRoles.Admin)]
    [Route("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _countryService.Delete(id);

            return Ok();
        }
        catch
        {
            throw new Exception("Could not delete Country.");
        }

    }
}