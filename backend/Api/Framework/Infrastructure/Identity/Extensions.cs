using TalentMesh.Framework.Core.Audit;
using TalentMesh.Framework.Core.Identity.Roles;
using TalentMesh.Framework.Core.Identity.Tokens;
using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Auth;
using TalentMesh.Framework.Infrastructure.Identity.Audit;
using TalentMesh.Framework.Infrastructure.Identity.Persistence;
using TalentMesh.Framework.Infrastructure.Identity.Roles;
using TalentMesh.Framework.Infrastructure.Identity.Roles.Endpoints;
using TalentMesh.Framework.Infrastructure.Identity.Tokens;
using TalentMesh.Framework.Infrastructure.Identity.Tokens.Endpoints;
using TalentMesh.Framework.Infrastructure.Identity.Users;
using TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
using TalentMesh.Framework.Infrastructure.Identity.Users.Services;
using TalentMesh.Framework.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

using IdentityConstants = TalentMesh.Shared.Authorization.IdentityConstants;

namespace TalentMesh.Framework.Infrastructure.Identity;
[ExcludeFromCodeCoverage]

internal static class Extensions
{
    internal static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddScoped<CurrentUserMiddleware>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IAuditService, AuditService>();
        services.BindDbContext<IdentityDbContext>();
        services.AddScoped<IDbInitializer, IdentityDbInitializer>();
        services.AddIdentity<TMUser, TMRole>(options =>
           {
               options.Password.RequiredLength = IdentityConstants.PasswordLength;
               options.Password.RequireDigit = false;
               options.Password.RequireLowercase = false;
               options.Password.RequireNonAlphanumeric = false;
               options.Password.RequireUppercase = false;
               options.User.RequireUniqueEmail = true;
           })
           .AddEntityFrameworkStores<IdentityDbContext>()
           .AddDefaultTokenProviders();
        return services;
    }
    [ExcludeFromCodeCoverage]

    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var users = app.MapGroup("api/users").WithTags("users");
        users.MapUserEndpoints();

        var tokens = app.MapGroup("api/token").WithTags("token");
        tokens.MapTokenEndpoints();

        var roles = app.MapGroup("api/roles").WithTags("roles");
        roles.MapRoleEndpoints();

        return app;
    }
}
