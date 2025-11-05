using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Light.Authorization;

public static class DependencyInjection
{
    public static IServiceCollection AddDefaultPermissionManager(this IServiceCollection services) =>
        services.AddSingleton<PermissionManager>();

    public static IServiceCollection AddPermissionManager<T>(this IServiceCollection services)
        where T : PermissionManager
    {
        return services.AddSingleton<PermissionManager, T>();
    }

    public static IServiceCollection AddPermissionPolicy(this IServiceCollection services) =>
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

    public static IServiceCollection AddPermissionAuthorization<T>(this IServiceCollection services)
        where T : class, IAuthorizationHandler
    {
        services.AddScoped<IAuthorizationHandler, T>();

        return services;
    }
        
}