using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Notifications.Application.Notifications.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Notifications.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class CreateNotificationEndpoint
{
    internal static RouteHandlerBuilder MapNotificationCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateNotificationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateNotificationEndpoint))
            .WithSummary("Notification")
            .WithDescription("Notification")
            .Produces<CreateNotificationResponse>()
            .RequirePermission("Permissions.Notifications.Create")
            .MapToApiVersion(1);
    }
}
