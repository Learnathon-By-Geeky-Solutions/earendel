using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
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
            // .RequirePermission("Permissions.Products.Create")
            .MapToApiVersion(1);
    }
}
