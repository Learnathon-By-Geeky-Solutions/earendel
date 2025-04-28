using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class DeleteQuizAttemptAnswerEndpoint
{
    internal static RouteHandlerBuilder MapQuizAttemptAnswerDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteQuizAttemptAnswerCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteQuizAttemptAnswerEndpoint))
            .WithSummary("deletes answer by id")
            .WithDescription("deletes answer by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.QuizAttemptAnswers.Delete")    // ← enforce Create permission
            .MapToApiVersion(1);
    }
}
