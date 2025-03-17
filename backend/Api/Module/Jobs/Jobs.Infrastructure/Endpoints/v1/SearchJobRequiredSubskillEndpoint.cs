using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Get.v1;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1
{
    public static class SearchJobRequiredSubskillEndpoint
    {
        internal static RouteHandlerBuilder MapGetJobRequiredSubskillListEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/search", async (ISender mediator, [FromBody] SearchJobRequiredSubskillCommand command) =>
                {
                    var response = await mediator.Send(command);
                    return Results.Ok(response);
                })
                .WithName(nameof(SearchJobRequiredSubskillEndpoint))
                .WithSummary("Gets a list of Job Required Subskills")
                .WithDescription("Gets a list of Job Required Subskills with pagination and filtering support")
                .Produces<PagedList<JobRequiredSubskillResponse>>()
                .RequirePermission("Permissions.JobRequiredSubskill.View")
                .MapToApiVersion(1);
        }
    }
}
