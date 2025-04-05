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
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Framework.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

using IdentityConstants = TalentMesh.Shared.Authorization.IdentityConstants;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Framework.Core.Jobs;
using TalentMesh.Framework.Core.Mail;
using Finbuckle.MultiTenant.Abstractions;
using TalentMesh.Framework.Infrastructure.Tenant;
using TalentMesh.Framework.Core.Storage;

namespace TalentMesh.Framework.Infrastructure.Identity
{
    [ExcludeFromCodeCoverage]
    internal static class Extensions
    {
        internal static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            // Register core middleware and services
            services.AddScoped<CurrentUserMiddleware>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());
            services.AddScoped<IDbInitializer, IdentityDbInitializer>();

            // Register TokenService (and other services assumed to be registered elsewhere)
            services.AddScoped<ITokenService, TokenService>();
            services.AddHostedService<UsersConsumer>();

            // Bind Identity DbContext
            services.BindDbContext<IdentityDbContext>();

            // Register Identity and configure options
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

            // Register custom parameter objects for UserService

            // IdentityServices groups identity-related dependencies.
            services.AddTransient<IdentityServices>(sp =>
            {
                return new IdentityServices(
                    sp.GetRequiredService<UserManager<TMUser>>(),
                    sp.GetRequiredService<SignInManager<TMUser>>(),
                    sp.GetRequiredService<RoleManager<TMRole>>(),
                    sp.GetRequiredService<IHttpContextAccessor>(),
                    sp.GetRequiredService<IdentityDbContext>(),
                    sp.GetRequiredService<IExternalApiClient>()
                );
            });

            // InfrastructureServices groups other dependencies.
            services.AddTransient<InfrastructureServices>(sp =>
            {
                return new InfrastructureServices(
                    sp.GetRequiredService<ICacheService>(),
                    sp.GetRequiredService<IJobService>(),
                    sp.GetRequiredService<IMailService>(),
                    sp.GetRequiredService<IMultiTenantContextAccessor<TMTenantInfo>>(),
                    sp.GetRequiredService<IStorageService>(),
                    sp.GetRequiredService<IMessageBus>(),
                    sp.GetRequiredService<ITokenService>()
                );
            });

            // Register UserService with its dependencies.
            services.AddTransient<IUserService, UserService>();
            services.AddHttpClient<IExternalApiClient, ExternalApiClient>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAuditService, AuditService>();

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
}
