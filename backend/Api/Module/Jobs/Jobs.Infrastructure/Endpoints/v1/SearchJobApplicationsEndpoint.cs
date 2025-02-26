using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobApplication.Get.v1;
using TalentMesh.Module.Job.Application.JobApplication.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1;
public static class SearchJobApplicationsEndpoint
{
    internal static RouteHandlerBuilder MapGetJobApplicationListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchJobApplicationsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchJobApplicationsEndpoint))
            .WithSummary("Gets a list of job applications")
            .WithDescription("Gets a list of job applications with pagination and filtering support")
            .Produces<PagedList<JobApplicationResponse>>()
            .RequirePermission("Permissions.JobApplications.View")
            .MapToApiVersion(1);
    }
}
