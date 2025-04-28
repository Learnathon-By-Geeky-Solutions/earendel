using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Notifications.Application.Notifications.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Notifications.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetNotificationEndpoint
{
    internal static RouteHandlerBuilder MapGetNotificationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetNotificationRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetNotificationEndpoint))
            .WithSummary("gets notification by id")
            .WithDescription("gets notification by id")
            .Produces<NotificationResponse>()
            .RequirePermission("Permissions.Notifications.View")
            .MapToApiVersion(1);
    }
}
