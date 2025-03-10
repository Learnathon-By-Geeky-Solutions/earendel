using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Core.Identity.Users.Features.RegisterUser;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Shared.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Framework.Infrastructure.Identity.Tokens.Endpoints;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
public static class GoogleLoginUserEndpoint
{
    internal static RouteHandlerBuilder MapGoogleLoginUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/google-login", (TokenRequestCommand request,
            // [FromHeader(Name = TenantConstants.Identifier)] string tenant,
            IUserService service,
            HttpContext context,
            CancellationToken cancellationToken,
            [FromHeader(Name = TenantConstants.Identifier)] string? tenant = "root") =>
        {
            string ip = context.GetIpAddress();
            var origin = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}";
            return service.GoogleLogin(request, ip, origin, cancellationToken);
            // return service.GoogleLogin(request, origin, cancellationToken);
        })
        
        .WithName(nameof(GoogleLoginUserEndpoint))
        .WithSummary("google login user")
        .RequirePermission("Permissions.Users.Create")
        .WithDescription("google login user")
        .AllowAnonymous();
    }
}
