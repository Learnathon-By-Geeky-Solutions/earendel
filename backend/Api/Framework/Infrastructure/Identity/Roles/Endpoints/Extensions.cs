using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Framework.Infrastructure.Identity.Roles.Endpoints;

internal static class Extensions
{
    public static IEndpointRouteBuilder MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetRoleEndpoint();
        app.MapGetRolesEndpoint();
        app.MapDeleteRoleEndpoint();
        app.MapCreateOrUpdateRoleEndpoint();
        app.MapGetRolePermissionsEndpoint();
        app.MapUpdateRolePermissionsEndpoint();
        return app;
    }
}

