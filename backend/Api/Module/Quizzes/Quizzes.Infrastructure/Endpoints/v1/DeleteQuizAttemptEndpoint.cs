using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class DeleteQuizAttemptEndpoint
{
    internal static RouteHandlerBuilder MapQuizAttemptDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteQuizAttemptCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteQuizAttemptEndpoint))
            .WithSummary("deletes Quiz by id")
            .WithDescription("deletes Quiz by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.QuizAttempts.Delete")    // ← enforce Create permission
            .MapToApiVersion(1);
    }
}
