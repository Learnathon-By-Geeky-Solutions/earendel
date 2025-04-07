using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Infrastructure.Persistence;

namespace TalentMesh.Module.CandidateLogic;

public static class JobViewEndpoint
{
    public static RouteHandlerBuilder MapJobViewEndpoints(this IEndpointRouteBuilder app)
    {
        return app.MapGet("/jobview", 
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
            .WithTags("JobView")
            .WithName("GetAllJobs For Candidates")
            .Produces<List<Jobs>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
    }
}
