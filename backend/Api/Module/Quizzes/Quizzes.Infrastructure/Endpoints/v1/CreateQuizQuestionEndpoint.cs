using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
public static class CreateQuizQuestionEndpoint
{
    internal static RouteHandlerBuilder MapQuizQuestionCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateQuizQuestionCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateQuizQuestionEndpoint))
            .WithSummary("creates a Question")
            .WithDescription("creates a Question")
            .Produces<CreateQuizQuestionResponse>()
            // .RequirePermission("Permissions.Products.Create")
            .MapToApiVersion(1);
    }
}
