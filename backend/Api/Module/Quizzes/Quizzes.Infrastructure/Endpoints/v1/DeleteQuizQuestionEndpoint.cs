using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class DeleteQuizQuestionEndpoint
{
    internal static RouteHandlerBuilder MapQuizQuestionDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteQuizQuestionCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteQuizQuestionEndpoint))
            .WithSummary("deletes Question by id")
            .WithDescription("deletes Question by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.QuizAttemptQuestions.Delete")    // ← enforce Create permission
            .MapToApiVersion(1);
    }
}
