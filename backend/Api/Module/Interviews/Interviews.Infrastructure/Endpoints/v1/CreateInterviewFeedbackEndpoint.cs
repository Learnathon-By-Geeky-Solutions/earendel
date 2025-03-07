using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
public static class CreateInterviewFeedbackEndpoint
{
    internal static RouteHandlerBuilder MapInterviewFeedbackCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInterviewFeedbackCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateInterviewEndpoint))
            .WithSummary("InterviewFeedback")
            .WithDescription("Interview Question")
            .Produces<CreateInterviewFeedbackResponse>()
            // .RequirePermission("Permissions.Products.Create")
            .MapToApiVersion(1);
    }
}
