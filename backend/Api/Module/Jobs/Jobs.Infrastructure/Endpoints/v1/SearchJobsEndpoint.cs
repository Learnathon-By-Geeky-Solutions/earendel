using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.Jobs.Get.v1;
using TalentMesh.Module.Job.Application.Jobs.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class SearchJobsEndpoint
{
    internal static RouteHandlerBuilder MapGetJobListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchJobsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchJobsEndpoint))
            .WithSummary("Gets a list of jobs")
            .WithDescription("Gets a list of jobs with pagination and filtering support")
            .Produces<PagedList<JobResponse>>()
            .RequirePermission("Permissions.Job.View")
            .MapToApiVersion(1);
    }
}

