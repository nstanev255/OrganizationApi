using OrganizationApi.Dto;

namespace OrganizationApi.Services;

public interface IAuthService
{
    Task<AuthenticationResponseModel> Register(RegisterRequestModel model, string role);
    Task<AuthenticationResponseModel> Login(LoginRequestModel model);
}