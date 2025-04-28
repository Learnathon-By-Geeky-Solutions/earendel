using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Get.v1;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class SearchQuizAttemptsEndpoint
{
    internal static RouteHandlerBuilder MapGetQuizAttemptListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchQuizAttemptsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchQuizAttemptsEndpoint))
            .WithSummary("Gets a list of Quiz")
            .WithDescription("Gets a list of Quiz with pagination and filtering support")
            .Produces<PagedList<QuizAttemptResponse>>()
            .RequirePermission("Permissions.QuizAttempts.View")
            .MapToApiVersion(1);
    }
}

