using System.Security.Claims;
using NETCleanArchDomain.Entities;

namespace NETCleanArchInfrastructure.ISecurity
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
