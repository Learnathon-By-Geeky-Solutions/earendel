using System.Reflection;
using Asp.Versioning.Conventions;
using FluentValidation;
using TalentMesh.Framework.Core;
using TalentMesh.Framework.Core.Origin;
using TalentMesh.Framework.Infrastructure.Auth;
using TalentMesh.Framework.Infrastructure.Auth.Jwt;
using TalentMesh.Framework.Infrastructure.Behaviours;
using TalentMesh.Framework.Infrastructure.Caching;
using TalentMesh.Framework.Infrastructure.Cors;
using TalentMesh.Framework.Infrastructure.Exceptions;
using TalentMesh.Framework.Infrastructure.Identity;
using TalentMesh.Framework.Infrastructure.Jobs;
using TalentMesh.Framework.Infrastructure.Logging.Serilog;
using TalentMesh.Framework.Infrastructure.Mail;
using TalentMesh.Framework.Infrastructure.Persistence;
//using TalentMesh.Framework.Infrastructure.RateLimit;
using TalentMesh.Framework.Infrastructure.SecurityHeaders;
using TalentMesh.Framework.Infrastructure.Storage.Files;
//using TalentMesh.Framework.Infrastructure.Tenant;
//using TalentMesh.Framework.Infrastructure.Tenant.Endpoints;
using TalentMesh.Aspire.ServiceDefaults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace TalentMesh.Framework.Infrastructure;

public static class Extensions
{
    public static WebApplicationBuilder ConfigureTMFramework(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.AddServiceDefaults();
        builder.ConfigureSerilog();
        builder.ConfigureDatabase();
        builder.Services.ConfigureIdentity();
        builder.Services.AddCorsPolicy(builder.Configuration);
        builder.Services.ConfigureFileStorage();
        builder.Services.ConfigureJwtAuth();
        //builder.Services.ConfigureOpenApi();
        builder.Services.ConfigureJobs(builder.Configuration);
        builder.Services.ConfigureMailing();
        builder.Services.ConfigureCaching(builder.Configuration);
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddHealthChecks();
        builder.Services.AddOptions<OriginOptions>().BindConfiguration(nameof(OriginOptions));

        // Define module assemblies
        var assemblies = new Assembly[]
        {
            typeof(TMCore).Assembly,
            typeof(TMInfrastructure).Assembly
        };

        // Register validators
        builder.Services.AddValidatorsFromAssemblies(assemblies);

        // Register MediatR
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        //builder.Services.ConfigureRateLimit(builder.Configuration);
        builder.Services.ConfigureSecurityHeaders(builder.Configuration);

        return builder;
    }

    public static WebApplication UseFshFramework(this WebApplication app)
    {
        app.MapDefaultEndpoints();
        //app.UseRateLimit();
        app.UseSecurityHeaders();
        //app.UseMultitenancy();
        app.UseExceptionHandler();
        app.UseCorsPolicy();
        //app.UseOpenApi();
        app.UseJobDashboard(app.Configuration);
        app.UseRouting();
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "assets")),
            RequestPath = new PathString("/assets")
        });
        app.UseAuthentication();
        app.UseAuthorization();
        //app.MapTenantEndpoints();
        app.MapIdentityEndpoints();

        // Current user middleware
        app.UseMiddleware<CurrentUserMiddleware>();

        // Register API versions
        var versions = app.NewApiVersionSet()
                    .HasApiVersion(1)
                    .HasApiVersion(2)
                    .ReportApiVersions()
                    .Build();

        // Map versioned endpoint
        app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(versions);

        return app;
    }
}
