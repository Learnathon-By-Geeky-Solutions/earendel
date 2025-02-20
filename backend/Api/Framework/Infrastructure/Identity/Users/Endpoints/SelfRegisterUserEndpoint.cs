using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Core.Identity.Users.Features.RegisterUser;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Shared.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
public static class SelfRegisterUserEndpoint
{
    internal static RouteHandlerBuilder MapSelfRegisterUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/self-register", (RegisterUserCommand request,
            [FromHeader(Name = TenantConstants.Identifier)] string tenant,
            IUserService service,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var origin = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}";
            return service.RegisterAsync(request, origin, cancellationToken);
        })
        .WithName(nameof(SelfRegisterUserEndpoint))
        .WithSummary("self register user")
        .RequirePermission("Permissions.Users.Create")
        .WithDescription("self register user")
        .AllowAnonymous();
    }
}
