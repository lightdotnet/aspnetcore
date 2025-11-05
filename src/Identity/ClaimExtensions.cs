using System.Security.Claims;

namespace Light.Identity;

public static class ClaimExtensions
{
    public static List<Claim> Add(this List<Claim> claims, string key, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            claims.Add(new Claim(key, value));
        }

        return claims;
    }

    public static List<Claim> Get(this UserDto user)
    {
        var claims = new List<Claim>()
        {
            { ClaimTypes.UserId, user.Id },
            { ClaimTypes.UserName, user.UserName },
            { ClaimTypes.FirstName, user.FirstName },
            { ClaimTypes.LastName, user.LastName },
            { ClaimTypes.PhoneNumber, user.PhoneNumber },
            { ClaimTypes.Email, user.Email },
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        foreach (var claim in user.Claims)
        {
            claims.Add(new Claim(claim.Type, claim.Value));
        }

        return claims;
    }
}
