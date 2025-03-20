using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Notifications.Application.Notifications.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Notifications.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class UpdateNotificationEndpoint
{
    internal static RouteHandlerBuilder MapNotificationUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateNotificationCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateNotificationEndpoint))
            .WithSummary("update Notification")
            .WithDescription("update Notification")
            .Produces<UpdateNotificationResponse>()
            // .RequirePermission("Permissions.Products.Update")
            .MapToApiVersion(1);
    }
}
