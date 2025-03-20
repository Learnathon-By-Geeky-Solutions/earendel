using TalentMesh.Framework.Core.Identity.Tokens;
using TalentMesh.Framework.Core.Identity.Tokens.Features.Refresh;
using TalentMesh.Shared.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Tokens.Endpoints;
[ExcludeFromCodeCoverage]

public static class RefreshTokenEndpoint
{
    internal static RouteHandlerBuilder MapRefreshTokenEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/refresh", (RefreshTokenCommand request,
            // [FromHeader(Name = TenantConstants.Identifier)] string tenant, 
            ITokenService service,
            HttpContext context,
            CancellationToken cancellationToken,
            [FromHeader(Name = TenantConstants.Identifier)] string? tenant = "root") =>
        {
            string ip = context.GetIpAddress();
            return service.RefreshTokenAsync(request, ip!, cancellationToken);
        })
        .WithName(nameof(RefreshTokenEndpoint))
        .WithSummary("refresh JWTs")
        .WithDescription("refresh JWTs")
        .AllowAnonymous();
    }
}
