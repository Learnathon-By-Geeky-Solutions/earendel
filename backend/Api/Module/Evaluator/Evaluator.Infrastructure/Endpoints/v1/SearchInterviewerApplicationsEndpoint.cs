using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Evaluator.Application.Interviewer.Get.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1;

namespace TalentMesh.Module.Evaluator.Infrastructure.Endpoints.v1
{
    public static class SearchInterviewerApplicationsEndpoint
    {
        internal static RouteHandlerBuilder MapGetInterviewerApplicationListEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/search", async (ISender mediator, [FromBody] SearchInterviewerApplicationsCommand command) =>
                {
                    var response = await mediator.Send(command);
                    return Results.Ok(response);
                })
                .WithName(nameof(SearchInterviewerApplicationsEndpoint))
                .WithSummary("Gets a list of Interviewer Applications")
                .WithDescription("Gets a list of Interviewer Applications with pagination and filtering support")
                .Produces<PagedList<InterviewerApplicationResponse>>()
                .RequirePermission("Permissions.Interviewer.View")
                .MapToApiVersion(1);
        }
    }
}
