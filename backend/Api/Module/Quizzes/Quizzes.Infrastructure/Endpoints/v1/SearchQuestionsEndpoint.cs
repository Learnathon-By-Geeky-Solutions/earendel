using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Get.v1;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]


public static class SearchQuizQuestionsEndpoint
{
    internal static RouteHandlerBuilder MapGetQuizQuestionListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchQuizQuestionsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchQuizQuestionsEndpoint))
            .WithSummary("Gets a list of Question")
            .WithDescription("Gets a list of Quetion with pagination and filtering support")
            .Produces<PagedList<QuizQuestionResponse>>()
            .RequirePermission("Permissions.QuizQuestions.View")
            .MapToApiVersion(1);
    }
}

