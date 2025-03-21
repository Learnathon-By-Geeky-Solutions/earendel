using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
[ExcludeFromCodeCoverage]

public static class GetUserEndpoint
{
    internal static RouteHandlerBuilder MapGetUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", (string id, IUserService service) =>
        {
            return service.GetAsync(id, CancellationToken.None);
        })
        .WithName(nameof(GetUserEndpoint))
        .WithSummary("Get user profile by ID")
        .RequirePermission("Permissions.Users.View")
        .WithDescription("Get another user's profile details by user ID.");
    }
}
