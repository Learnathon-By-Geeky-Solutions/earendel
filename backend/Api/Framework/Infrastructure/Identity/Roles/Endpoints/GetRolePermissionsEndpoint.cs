using TalentMesh.Framework.Core.Identity.Roles;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Roles.Endpoints;
[ExcludeFromCodeCoverage]

public static class GetRolePermissionsEndpoint
{
    public static RouteHandlerBuilder MapGetRolePermissionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}/permissions", async (string id, IRoleService roleService, CancellationToken cancellationToken) =>
        {
            return await roleService.GetWithPermissionsAsync(id, cancellationToken);
        })
        .WithName(nameof(GetRolePermissionsEndpoint))
        .WithSummary("get role permissions")
        .RequirePermission("Permissions.Roles.View")
        .WithDescription("get role permissions");
    }
}
