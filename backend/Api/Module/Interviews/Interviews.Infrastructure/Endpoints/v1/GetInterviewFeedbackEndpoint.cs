using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetInterviewFeedbackEndpoint
{
    internal static RouteHandlerBuilder MapGetInterviewFeedbackEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInterviewFeedbackRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetInterviewFeedbackEndpoint))
            .WithSummary("gets Interview by id")
            .WithDescription("gets Interview by id")
            .Produces<InterviewFeedbackResponse>()
            .RequirePermission("Permissions.InterviewFeedbacks.View")
            .MapToApiVersion(1);
    }
}
