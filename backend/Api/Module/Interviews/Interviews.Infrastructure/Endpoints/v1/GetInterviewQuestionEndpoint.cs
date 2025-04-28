using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetInterviewQuestionEndpoint
{
    internal static RouteHandlerBuilder MapGetInterviewQuestionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInterviewQuestionRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetInterviewQuestionEndpoint))
            .WithSummary("gets InterviewQuestion by id")
            .WithDescription("gets InterviewQuestion by id")
            .Produces<InterviewQuestionResponse>()
            .RequirePermission("Permissions.InterviewQuestions.View")
            .MapToApiVersion(1);
    }
}
