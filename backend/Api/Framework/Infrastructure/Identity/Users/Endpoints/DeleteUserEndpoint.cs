using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
public static class DeleteUserEndpoint
{
    internal static RouteHandlerBuilder MapDeleteUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", (string id, IUserService service) =>
        {
            return service.DeleteAsync(id);
        })
        .WithName(nameof(DeleteUserEndpoint))
        .WithSummary("delete user profile")
        .RequirePermission("Permissions.Users.Delete")
        .WithDescription("delete user profile");
    }
}
