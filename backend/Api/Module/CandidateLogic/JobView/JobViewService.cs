using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TalentMesh.Module.Job.Infrastructure.Persistence;

namespace TalentMesh.Module.CandidateLogic.JobView;


public class JobViewService : IRequestHandler<JobViewFilters, IResult>
{
    private readonly JobDbContext _job_context;

    public JobViewService(JobDbContext context)
    {
        _job_context = context;
    }

    public async Task<IResult> Handle(JobViewFilters request, CancellationToken cancellationToken)
    {
        var query = _job_context.Jobs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(j => j.Name.Contains(request.Name));

        if (!string.IsNullOrWhiteSpace(request.Description))
            query = query.Where(j => j.Description != null && j.Description.Contains(request.Description));

        if (!string.IsNullOrWhiteSpace(request.Requirements))
            query = query.Where(j => j.Requirments.Contains(request.Requirements));

        if (!string.IsNullOrWhiteSpace(request.Location))
            query = query.Where(j => j.Location.Contains(request.Location));

        if (!string.IsNullOrWhiteSpace(request.JobType))
            query = query.Where(j => j.JobType.Contains(request.JobType));

        if (!string.IsNullOrWhiteSpace(request.ExperienceLevel))
            query = query.Where(j => j.ExperienceLevel.Contains(request.ExperienceLevel));

        var results = await query.ToListAsync(cancellationToken);
        return Results.Ok(results);
    }
}

