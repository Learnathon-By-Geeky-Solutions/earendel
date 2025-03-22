using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Get.v1;

namespace TalentMesh.Module.CandidateLogic;

public static class JobViewEndpoint
{
    internal static RouteHandlerBuilder MapJobViewEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/api/jobs", async (IMediator mediator, [AsParameters] JobViewFilters query) =>
            {
                var jobs = await mediator.Send(query);
                return Results.Ok(jobs);
            })
            .WithName(nameof(JobViewEndpoint))
            .WithSummary("Get Jobs With Filters")
            .WithDescription("Get Job With Multiple Filters availavle")
            //.Produces<CandidateProfileResponse>()
            .RequirePermission("Permissions.Job.View")
            .MapToApiVersion(1);
    }
}