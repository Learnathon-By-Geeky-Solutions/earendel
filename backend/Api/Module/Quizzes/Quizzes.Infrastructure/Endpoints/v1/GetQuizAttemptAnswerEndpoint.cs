using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetQuizAttemptAnswerEndpoint
{
    internal static RouteHandlerBuilder MapGetQuizAttemptAnswerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetQuizAttemptAnswerRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetQuizAttemptAnswerEndpoint))
            .WithSummary("gets attempt answer by id")
            .WithDescription("gets answer by id")
            .Produces<QuizAttemptAnswerResponse>()
            // .RequirePermission("Permissions.Jobs.View")
            .MapToApiVersion(1);
    }
}
