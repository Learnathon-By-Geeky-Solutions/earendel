using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Get.v1;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class SearchInterviewQuestionsEndpoint
{
    internal static RouteHandlerBuilder MapGetInterviewQuestionListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInterviewQuestionsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInterviewQuestionsEndpoint))
            .WithSummary("Gets a list of InterviewQuestion")
            .WithDescription("Gets a list of InterviewQuestion with pagination and filtering support")
            .Produces<PagedList<InterviewQuestionResponse>>()
            .RequirePermission("Permissions.InterviewQuestions.View")
            .MapToApiVersion(1);
    }
}

