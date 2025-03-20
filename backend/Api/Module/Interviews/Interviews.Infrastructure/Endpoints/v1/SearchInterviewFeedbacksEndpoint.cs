using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Get.v1;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class SearchInterviewFeedbacksEndpoint
{
    internal static RouteHandlerBuilder MapGetInterviewFeedbackListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInterviewFeedbacksCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInterviewFeedbacksEndpoint))
            .WithSummary("Gets a list of Interview")
            .WithDescription("Gets a list of Interview with pagination and filtering support")
            .Produces<PagedList<InterviewFeedbackResponse>>()
            // .RequirePermission("Permissions.Products.View")
            .MapToApiVersion(1);
    }
}

