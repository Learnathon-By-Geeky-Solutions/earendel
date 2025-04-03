using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
[ExcludeFromCodeCoverage]

public static class GetAdminsEndpoint
{
    internal static RouteHandlerBuilder MapGetAdminsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/admin", (IUserService service) =>
        {
            return service.GetAdminsAsync(CancellationToken.None);
        })
        .WithName(nameof(GetAdminsEndpoint))
        .WithSummary("Get Admin List")
        // .RequirePermission("Permissions.Users.View")
        .WithDescription("Get Admin List.");
    }
}
