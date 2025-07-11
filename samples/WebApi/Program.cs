global using Light.AspNetCore.Mvc;
using FluentValidation;
using Light.AspNetCore.Builder;
using Light.AspNetCore.Middlewares;
using Light.AspNetCore.Swagger;
using Light.Caching.Infrastructure;
using Light.Extensions.DependencyInjection;
using Light.Identity;
using Light.Serilog;
using Serilog;
using System.Reflection;
using WebApi.HealthChecks;
using WebApi.Identity;
using WebApi.SoapCore;
using WebApi.TestOption;

Serilogger.EnsureInitialized();
Log.Information("Application start...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.LoadConfigurationFrom(["configurations"]);
    //builder.AddConfigurations(["D:", "configurations"]);
    //builder.AddJsonFiles();
    builder.Host.ConfigureSerilog();

    // Add services to the container.

    var executingAssembly = Assembly.GetExecutingAssembly();

    var settings = builder.Configuration.GetSection("Caching").Get<CacheOptions>();
    builder.Services.AddCache(opt =>
    {
        opt.Provider = settings!.Provider;
        opt.RedisHost = settings.RedisHost;
        opt.RedisPassword = settings.RedisPassword;
    });

    builder.Services.AddTestOptions(builder.Configuration);

    // Overide by BindConfiguration
    var issuer = builder.Configuration.GetValue<string>("JWT:Issuer");
    var key = builder.Configuration.GetValue<string>("JWT:SecretKey");
    builder.Services.AddJwtAuth(issuer!, key!, ClaimTypes.Role);

    //builder.Services.AddTelegram();

    builder.Services.AddOptions<RequestLoggingOptions>().BindConfiguration("RequestLogging");

    builder.Services
        .AddControllers()
        .AddInvalidModelStateHandler()
        .AddDefaultJsonOptions();

    builder.Services.AddApiVersion(1, 0, false);
    builder.Services.AddSwagger(builder.Configuration);

    //builder.Services.AddGlobalExceptionHandler();

    builder.Services.AutoAddDependencies();

    builder.Services.AddInfrastructureIdentity(builder.Configuration);

    builder.Services.AddModules(builder.Configuration, [executingAssembly]);

    builder.Services.AddAppSoapCore();

    builder.Services.AddValidatorsFromAssemblies([executingAssembly]);

    builder.Services.AddHealthChecksService();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSwagger();

    app.UseUlidTraceId();
    //app.UseMiddlewares(builder.Configuration);
    app.UseLightRequestLogging();
    app.UseLightExceptionHandler(); // must inject after Inbound Logging

    //app.UseExceptionHandler();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAppSoapCore();

    app.UseModules([executingAssembly]);

    app.MapControllers();

    app.MapHealthChecksEndpoint();

    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    Serilogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete.");
    Log.CloseAndFlush();
}