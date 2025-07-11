using Light.Identity;
using Light.Identity.Options;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Identity;

public static class Startup
{
    /// <summary>
    /// Config Identity data
    /// </summary>
    public static IServiceCollection AddInfrastructureIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStr = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionStr));

        services.AddIdentity<AppIdentityDbContext>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;

            // Password settings
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 3;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;

            // Lockout settings
            //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
            //options.Lockout.MaxFailedAccessAttempts = 10;

            // User settings
            options.User.RequireUniqueEmail = false;
        });

        var jwtSettings = configuration.GetSection("JWT").Get<JwtOptions>();

        ArgumentNullException.ThrowIfNull(jwtSettings, nameof(JwtOptions));

        services.AddJwtTokenProvider(opt =>
        {
            opt.Issuer = jwtSettings.Issuer;
            opt.SecretKey = jwtSettings.SecretKey;
            opt.AccessTokenExpirationSeconds = jwtSettings.AccessTokenExpirationSeconds;
            opt.RefreshTokenExpirationDays = jwtSettings.RefreshTokenExpirationDays;
        });

        return services;
    }
}
