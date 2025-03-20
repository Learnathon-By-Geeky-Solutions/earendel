using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Notifications.Application.Notifications.Get.v1;
using TalentMesh.Module.Notifications.Application.Notifications.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Notifications.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class SearchNotificationsEndpoint
{
    internal static RouteHandlerBuilder MapGetNotificationListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchNotificationsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchNotificationsEndpoint))
            .WithSummary("Gets a list of Notification")
            .WithDescription("Gets a list of Notification with pagination and filtering support")
            .Produces<PagedList<NotificationResponse>>()
            // .RequirePermission("Permissions.Products.View")
            .MapToApiVersion(1);
    }
}

