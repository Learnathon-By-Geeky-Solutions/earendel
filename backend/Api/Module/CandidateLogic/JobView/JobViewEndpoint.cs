using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.CandidateLogic;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Infrastructure.Persistence;

namespace TalentMesh.Module.CandidateLogic.JobView;

[ExcludeFromCodeCoverage]
public static class JobViewEndpoint
{
    public static RouteHandlerBuilder MapJobViewEndpoints(this IEndpointRouteBuilder app)
    {
        return app.MapGet("JobView",
            async (
                IMediator mediator,
                [FromQuery] string? name,
                [FromQuery] string? description,
                [FromQuery] string? requirements,
                [FromQuery] string? location,
                [FromQuery] string? jobType,
                [FromQuery] string? experienceLevel) =>
        {
            var query = new JobViewFilters(
                name,
                description,
                requirements,
                location,
                jobType,
                experienceLevel);

            return await mediator.Send(query);
        })
            .WithTags("CandidateJobView")
            .WithName("GetAllJobsForCandidates")
            .Produces<List<Jobs>>(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Jobs.View")   // ← added correct permission
            .Produces(StatusCodes.Status500InternalServerError);
    }
}
