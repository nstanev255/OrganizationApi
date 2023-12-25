using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationApi.Dto;
using OrganizationApi.Dto.Jwt;
using OrganizationApi.Services;

namespace OrganizationApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequestModel model)
    {
        if (!ModelState.IsValid)
            throw new Exception("Invalid request");

        return Ok(await _authService.Login(model));
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterRequestModel model)
    {
        if (!ModelState.IsValid)
            throw new Exception("Invalid request");

        return Ok(await _authService.Register(model, UserRoles.User));
    }
}