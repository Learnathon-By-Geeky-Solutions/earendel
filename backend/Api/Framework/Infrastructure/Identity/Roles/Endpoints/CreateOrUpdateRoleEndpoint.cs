using TalentMesh.Framework.Core.Identity.Roles;
using TalentMesh.Framework.Core.Identity.Roles.Features.CreateOrUpdateRole;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Framework.Infrastructure.Identity.Roles.Endpoints;

public static class CreateOrUpdateRoleEndpoint
{
    public static RouteHandlerBuilder MapCreateOrUpdateRoleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateOrUpdateRoleCommand request, IRoleService roleService) =>
        {
            return await roleService.CreateOrUpdateRoleAsync(request);
        })
        .WithName(nameof(CreateOrUpdateRoleEndpoint))
        .WithSummary("Create or update a role")
        .RequirePermission("Permissions.Roles.Create")
        .WithDescription("Create a new role or update an existing role.");
    }
}
