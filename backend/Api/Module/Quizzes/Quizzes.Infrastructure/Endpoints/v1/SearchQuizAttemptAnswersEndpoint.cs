using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Get.v1;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class SearchQuizAttemptAnswersEndpoint
{
    internal static RouteHandlerBuilder MapGetQuizAttemptAnswerListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchQuizAttemptAnswersCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchQuizAttemptAnswersEndpoint))
            .WithSummary("Gets a list of Attempt Answer")
            .WithDescription("Gets a list of Attempt Answer with pagination and filtering support")
            .Produces<PagedList<QuizAttemptAnswerResponse>>()
            .RequirePermission("Permissions.QuizAttemptAnswers.View")
            .MapToApiVersion(1);
    }
}

