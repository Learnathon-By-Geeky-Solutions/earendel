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
    public static class SearchInterviewerAvailabilitiesEndpoint
    {
        internal static RouteHandlerBuilder MapGetInterviewerAvailabilityListEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/search", async (ISender mediator, [FromBody] SearchInterviewerAvailabilitiesCommand command) =>
                {
                    var response = await mediator.Send(command);
                    return Results.Ok(response);
                })
                .WithName(nameof(SearchInterviewerAvailabilitiesEndpoint))
                .WithSummary("Gets a list of Interviewer Availabilities")
                .WithDescription("Gets a list of Interviewer Availabilities with pagination and filtering support")
                .Produces<PagedList<InterviewerAvailabilityResponse>>()
                //  .RequirePermission("Permissions.Interviewer.View")
                .MapToApiVersion(1);
        }
    }
}
