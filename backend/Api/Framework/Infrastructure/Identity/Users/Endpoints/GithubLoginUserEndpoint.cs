using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Core.Identity.Users.Features.RegisterUser;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Shared.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Framework.Infrastructure.Identity.Tokens.Endpoints;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
[ExcludeFromCodeCoverage]

public static class GithubLoginUserEndpoint
{
    internal static RouteHandlerBuilder MapGithubLoginUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/github-login", (TokenRequestCommand request,
            // [FromHeader(Name = TenantConstants.Identifier)] string tenant,
            IUserService service,
            HttpContext context,
            CancellationToken cancellationToken,
            [FromHeader(Name = TenantConstants.Identifier)] string? tenant = "root") =>
        {
            string ip = context.GetIpAddress();
            var origin = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}";
            return service.GithubLogin(request, ip, origin, cancellationToken);
        })

        .WithName(nameof(GithubLoginUserEndpoint))
        .WithSummary("github login user")
        .RequirePermission("Permissions.Users.Create")
        .WithDescription("github login user")
        .AllowAnonymous();
    }
}
