using TalentMesh.Framework.Core.Identity.Tokens;
using TalentMesh.Framework.Core.Identity.Tokens.Features.Logout;
using TalentMesh.Shared.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Tokens.Endpoints;

[ExcludeFromCodeCoverage]
public static class LogoutEndpoint
{
    internal static RouteHandlerBuilder MapLogoutEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/logout", (LogoutCommand request,
            ITokenService service,
            CancellationToken cancellationToken,
            [FromHeader(Name = TenantConstants.Identifier)] string? tenant = "root") =>
        {
            return service.LogoutAsync(request, cancellationToken);
        })
        .WithName(nameof(LogoutEndpoint))
        .WithSummary("logout user")
        .WithDescription("invalidates the user's refresh token")
        .RequireAuthorization(); // Require authentication to logout
    }
}