using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Evaluator.Application.Interviewer.Get.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]
    public static class SearchInterviewerEntryFormsEndpoint
    {
        internal static RouteHandlerBuilder MapGetInterviewerEntryFormListEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/search", async (ISender mediator, [FromBody] SearchInterviewerEntryFormsCommand command) =>
                {
                    var response = await mediator.Send(command);
                    return Results.Ok(response);
                })
                .WithName(nameof(SearchInterviewerEntryFormsEndpoint))
                .WithSummary("Gets a list of Interviewer Entry Forms")
                .WithDescription("Gets a list of Interviewer Entry Forms with pagination and filtering support")
                .Produces<PagedList<InterviewerEntryFormResponse>>()
                 .RequirePermission("Permissions.Interviewer.View")
                .MapToApiVersion(1);
        }
    }
}
