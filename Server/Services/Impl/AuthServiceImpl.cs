using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExcelDataReader.Log;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OrganizationApi.Dto;
using OrganizationApi.Dto.Jwt;
using OrganizationApi.Entity;

namespace OrganizationApi.Services.Impl;

public class AuthServiceImpl : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthServiceImpl(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<AuthenticationResponseModel> Register(RegisterRequestModel model, string role)
    {
        var exists = await _userManager.FindByNameAsync(model.Username);

        if (exists != null)
        {
            throw new Exception("Error with the registration.");
        }

        var existsEmail = await _userManager.FindByEmailAsync(model.Email);
        if (existsEmail != null)
        {
            throw new Exception("Error with the registration.");
        }

        var user = new User
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
        };

        var createResult = await _userManager.CreateAsync(user, model.Password);

        if (!createResult.Succeeded)
        {
            throw new Exception("Error creating user. " + createResult.Errors.First().Description);
        }

        if (!await _roleManager.RoleExistsAsync(role))
            await _roleManager.CreateAsync(new IdentityRole(role));
        
        
        if (await _roleManager.RoleExistsAsync(UserRoles.User))
            await _userManager.AddToRoleAsync(user, role);
        
        // If we got here, this means that we have registered the user accordingly.
        // We will "Login", or "Generate the JWT token" so that we can be logged in on register if needed.
        var token = await GenerateToken(user);
        return  new AuthenticationResponseModel
        {
            Username = user.UserName,
            Email = user.Email,
            Token = token
        };
    }

    public async Task<AuthenticationResponseModel> Login(LoginRequestModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
            throw new Exception("Error logging in user.");
        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            throw new Exception("Error logging in user. h");

        var token = await GenerateToken(user);

        return new AuthenticationResponseModel
        {
            Username = user.UserName,
            Email = user.Email,
            Token = token
        };
    }

    /**
     * This method will generate a token for user.
     */
    private async Task<string> GenerateToken(User user)
    {
        // We will extract the roles.
        var roles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        // We will generate the token.
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(authClaims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }
}