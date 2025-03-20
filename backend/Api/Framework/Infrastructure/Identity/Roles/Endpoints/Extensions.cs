using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Roles.Endpoints;

internal static class Extensions
{
    [ExcludeFromCodeCoverage]

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

