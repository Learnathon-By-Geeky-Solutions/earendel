using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TalentMesh.Module.Job.Infrastructure.Persistence; // Use the correct namespace for your DbContext

namespace TalentMesh.Module.CandidateLogic.JobApplicationView // Or your preferred namespace
{
    [ExcludeFromCodeCoverage]
    public class JobApplicationViewService : IRequestHandler<JobApplicationViewFilters, IResult>
    {
        private readonly JobDbContext _jobDbContext;

        public JobApplicationViewService(JobDbContext context)
        {
            _jobDbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(JobApplicationViewFilters request, CancellationToken cancellationToken)
        {
            var query = _jobDbContext.JobApplications.AsQueryable(); // Use JobApplications DbSet

            if (request.JobId.HasValue)
                query = query.Where(app => app.JobId == request.JobId.Value);

            if (request.CandidateId.HasValue)
                query = query.Where(app => app.CandidateId == request.CandidateId.Value); // Filter by CandidateId (UserId)

            if (request.ApplicationDateStart.HasValue)
                query = query.Where(app => app.ApplicationDate >= request.ApplicationDateStart.Value);

            if (request.ApplicationDateEnd.HasValue)
                query = query.Where(app => app.ApplicationDate <= request.ApplicationDateEnd.Value);

            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(app => app.Status.Equals(request.Status, StringComparison.OrdinalIgnoreCase));

            var results = await query
                // Add any sorting if required, e.g., .OrderByDescending(app => app.ApplicationDate)
                .ToListAsync(cancellationToken);

            // You might want to map the results to a DTO (Data Transfer Object)
            // instead of returning the domain entity directly.
            return Results.Ok(results);
        }
    }
}