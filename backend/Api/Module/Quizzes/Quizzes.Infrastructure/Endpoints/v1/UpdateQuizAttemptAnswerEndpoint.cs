using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class UpdateQuizAttemptAnswerEndpoint
{
    internal static RouteHandlerBuilder MapQuizAttemptAnswerUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateQuizAttemptAnswerCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateQuizAttemptAnswerEndpoint))
            .WithSummary("update a Attempt Answer")
            .WithDescription("update a Attempt Answer")
            .Produces<UpdateQuizAttemptAnswerResponse>()
            .RequirePermission("Permissions.QuizAttemptAnswers.Update")
            .MapToApiVersion(1);
    }
}
