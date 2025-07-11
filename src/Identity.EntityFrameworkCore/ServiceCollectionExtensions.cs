﻿using Light.Identity.EntityFrameworkCore;
using Light.Identity.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Light.Identity;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Config default identity
    /// </summary>
    public static IServiceCollection AddIdentity<TContext>(this IServiceCollection services)
        where TContext : IdentityContext
    {
        services
            .AddIdentity<TContext>(options =>
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

        return services;
    }

    /// <summary>
    /// Config identity stores
    /// </summary>
    public static IServiceCollection AddIdentity<TContext>(this IServiceCollection services, Action<IdentityOptions> options)
        where TContext : IdentityContext
    {
        services
            .AddDefaultIdentity<User>(options)
            .AddRoles<Role>()
            .AddEntityFrameworkStores<TContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IIdentityContext>(provider => provider.GetRequiredService<TContext>());

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();

        return services;
    }

    /// <summary>
    /// Register Jwt Token services
    /// </summary>
    public static IServiceCollection AddJwtTokenProvider(this IServiceCollection services, Action<JwtOptions>? action = null)
    {
        if (action != null)
        {
            services.Configure(action);
        }

        services.AddScoped<JwtTokenMananger>();

        return services;
    }
}