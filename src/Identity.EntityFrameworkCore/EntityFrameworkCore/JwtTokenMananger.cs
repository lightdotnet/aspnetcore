using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Light.Identity.EntityFrameworkCore;

public class JwtTokenMananger(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IIdentityContext context)
{
    public UserManager<User> UserManager => userManager;

    public virtual DateTimeOffset TimeNow => DateTimeOffset.Now;

    public virtual Task<IEnumerable<Claim>> GetUserClaimsAsync(User user) =>
        new UserClaimProvider(userManager, roleManager).GetUserClaimsAsync(user);

    public virtual async Task<IResult<TokenDto>> GenerateTokenByAsync(
        User user,
        string issuer, string secretKey,
        DateTime tokenExpiresAt, DateTime refreshTokenExpiresAt,
        string? deviceId = null, string? deviceName = null)
    {
        var claims = await GetUserClaimsAsync(user);

        var jwtToken = JwtHelper.GenerateToken(
            issuer,
            claims,
            tokenExpiresAt,
            secretKey);

        var newToken = new JwtToken
        {
            UserId = user.Id,
            DeviceId = deviceId,
            DeviceName = deviceName,
            Token = jwtToken,
            TokenExpiresAt = tokenExpiresAt,
            RefreshToken = JwtHelper.GenerateRefreshToken(),
            RefreshTokenExpiresAt = refreshTokenExpiresAt,
        };

        await context.JwtTokens.AddAsync(newToken);
        await context.SaveChangesAsync();

        return Result<TokenDto>.Success(
            new TokenDto(
                newToken.Token,
                newToken.TokenExpiresInSeconds,
                newToken.RefreshToken));
    }

    public virtual async Task<IResult<TokenDto>> RefreshTokenAsync(
        User user,
        string refreshToken,
        string issuer, string secretKey,
        DateTime tokenExpiresAt, DateTime refreshTokenExpiresAt,
        string roleClaimType = ClaimTypes.Role, string userIdClaimType = ClaimTypes.UserId)
    {
        // check refresh token is exist and not out of lifetime
        var userToken = await context.JwtTokens
            .Where(x =>
                x.UserId == user.Id
                && x.RefreshToken == refreshToken
                && x.RefreshTokenExpiresAt >= TimeNow.Date
                && x.Revoked == false)
            .FirstOrDefaultAsync();

        if (userToken is null)
            return Result<TokenDto>.Unauthorized("Refresh token invalid.");

        var claims = await GetUserClaimsAsync(user);

        var timeNow = TimeNow.DateTime;

        var jwtToken = JwtHelper.GenerateToken(
            issuer,
            claims,
            tokenExpiresAt,
            secretKey);

        // save token data
        userToken.Token = jwtToken;
        userToken.TokenExpiresAt = tokenExpiresAt;
        userToken.RefreshToken = JwtHelper.GenerateRefreshToken();
        userToken.RefreshTokenExpiresAt = refreshTokenExpiresAt;

        await context.SaveChangesAsync();

        return Result<TokenDto>.Success(
            new TokenDto(
                userToken.Token,
                userToken.TokenExpiresInSeconds,
                userToken.RefreshToken));
    }

    public async Task<IEnumerable<UserTokenDto>> GetUserTokensAsync(string userId)
    {
        var now = TimeNow;

        var list = await context.JwtTokens
            .Where(x =>
                x.UserId == userId
                &&
                    (x.TokenExpiresAt >= now
                    || (x.RefreshTokenExpiresAt.HasValue && x.RefreshTokenExpiresAt >= now))
                && x.Revoked == false)
            .AsNoTracking()
            .Select(s => new UserTokenDto
            {
                Id = s.Id,
                ExpiresAt = s.TokenExpiresAt,
                RefreshTokenExpiresAt = s.RefreshTokenExpiresAt,
                DeviceId = s.DeviceId,
                DeviceName = s.DeviceName,
            })
            .ToListAsync();

        return list;
    }

    public Task RevokedAsync(string userId, string tokenId)
    {
        return context.JwtTokens
            .Where(x => x.Id == tokenId && x.UserId == userId)
            .ExecuteUpdateAsync(e => e.SetProperty(p => p.Revoked, true));
    }
}
