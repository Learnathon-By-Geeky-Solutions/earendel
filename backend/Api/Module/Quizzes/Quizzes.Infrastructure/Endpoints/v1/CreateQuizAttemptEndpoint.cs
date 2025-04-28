using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class CreateQuizAttemptEndpoint
{
    internal static RouteHandlerBuilder MapQuizAttemptCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateQuizAttemptCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateQuizAttemptEndpoint))
            .WithSummary("creates a Quiz")
            .WithDescription("creates a Quiz")
            .Produces<CreateQuizAttemptResponse>()
            .RequirePermission("Permissions.QuizAttempts.Create")    // ← enforce Create permission
            .MapToApiVersion(1);
    }
}
