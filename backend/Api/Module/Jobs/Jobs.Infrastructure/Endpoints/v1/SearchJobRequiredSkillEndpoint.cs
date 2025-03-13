using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Get.v1;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1
{
    public static class SearchJobRequiredSkillEndpoint
    {
        internal static RouteHandlerBuilder MapGetJobRequiredSkillListEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/search", async (ISender mediator, [FromBody] SearchJobRequiredSkillCommand command) =>
                {
                    var response = await mediator.Send(command);
                    return Results.Ok(response);
                })
                .WithName(nameof(SearchJobRequiredSkillEndpoint))
                .WithSummary("Gets a list of Job Required Skills")
                .WithDescription("Gets a list of Job Required Skills with pagination and filtering support")
                .Produces<PagedList<JobRequiredSkillResponse>>()
                .RequirePermission("Permissions.JobRequiredSkill.View")
                .MapToApiVersion(1);
        }
    }
}
