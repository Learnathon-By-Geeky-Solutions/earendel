using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetInterviewEndpoint
{
    internal static RouteHandlerBuilder MapGetInterviewEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInterviewRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetInterviewEndpoint))
            .WithSummary("gets Interview by id")
            .WithDescription("gets Interview by id")
            .Produces<InterviewResponse>()
            .RequirePermission("Permissions.Interviews.View")
            .MapToApiVersion(1);
    }
}
