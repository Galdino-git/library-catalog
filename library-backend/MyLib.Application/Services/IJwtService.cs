using System.Security.Claims;

namespace MyLib.Application.Services;

public interface IJwtService
{
    string GenerateToken(string userId, string username, string email);
    ClaimsPrincipal? ValidateToken(string token);
}