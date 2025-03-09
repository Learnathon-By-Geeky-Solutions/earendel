using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
public static class GetQuizQuestionEndpoint
{
    internal static RouteHandlerBuilder MapGetQuizQuestionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetQuizQuestionRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetQuizQuestionEndpoint))
            .WithSummary("gets Question by id")
            .WithDescription("gets Question by id")
            .Produces<QuizQuestionResponse>()
            // .RequirePermission("Permissions.Jobs.View")
            .MapToApiVersion(1);
    }
}
