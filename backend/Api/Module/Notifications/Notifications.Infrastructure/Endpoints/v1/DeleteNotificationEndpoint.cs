using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Notifications.Application.Notifications.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Notifications.Infrastructure.Endpoints.v1;
public static class DeleteNotificationEndpoint
{
    internal static RouteHandlerBuilder MapNotificationDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteNotificationCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteNotificationEndpoint))
            .WithSummary("deletes answer by id")
            .WithDescription("deletes answer by id")
            .Produces(StatusCodes.Status204NoContent)
            // .RequirePermission("Permissions.Products.Delete")
            .MapToApiVersion(1);
    }
}
