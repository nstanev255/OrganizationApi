using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

    [HttpGet]
    [Route("")]
    public List<CountryResponseModel> ReadAll(int page = 1, int pageSize = 10)
    {
        var countries = _countryService.ReadAll(page, pageSize);

        if (countries.IsNullOrEmpty())
        {
            throw new Exception("No records found");
        }


        return new List<CountryResponseModel>(countries.Select(c => new CountryResponseModel(c)));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<CountryResponseModel> Read(int id)
    {
        var country = await _countryService.Read(id.ToString());
        if (country == null)
        {
            throw new Exception("Country not found");
        }

        return new CountryResponseModel(country);
    }

    [HttpPost]
    [Authorize]
    [Route("")]
    public async Task<CountryResponseModel> Create(CountryModel model)
    {
        var entity = await _countryService.Create(new Country { Name = model.Name });
        return new CountryResponseModel(entity);
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    public async Task<CountryResponseModel> Update(int id, CountryModel model)
    {
        var entity = await _countryService.UpdateOrThrow(id, model);

        return new CountryResponseModel(entity);
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