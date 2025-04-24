using MediatR;
using Microsoft.AspNetCore.Http; // For IResult and Results
using Microsoft.EntityFrameworkCore; // For EF Core methods like Include, Skip, Take
using TalentMesh.Module.Job.Infrastructure.Persistence; // Your DbContext namespace
using TalentMesh.Module.Job.Domain; // Add this to access the Jobs entity

namespace TalentMesh.Module.HRView.HRFunc
{
    public class GetMyJobsQueryHandler : IRequestHandler<GetMyJobsQuery, IResult>
    {
        private readonly JobDbContext _context;

        public GetMyJobsQueryHandler(JobDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(GetMyJobsQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
            int pageSize = request.PageSize > 0 ? request.PageSize : 1000;

            // Query jobs posted by the requesting user
            var query = _context.Jobs
                .AsNoTracking()
                .Where(j => j.PostedById == request.RequestingUserId)
                .OrderByDescending(j => j.Created); // Most recent jobs first


            var jobs = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(job => new JobDto
                {
                    Id = job.Id,
                    Name = job.Name,
                    Description = job.Description ?? string.Empty,
                    Requirements = job.Requirments,
                    Location = job.Location,
                    JobType = job.JobType,
                    ExperienceLevel = job.ExperienceLevel,
                    Salary = job.Salary!,
                    PaymentStatus = job.PaymentStatus,
                    PostedById = job.PostedById,
                    CreatedOn = job.Created,
                    LastModifiedOn = job.LastModified
                })
                .ToListAsync(cancellationToken);

            return Results.Ok(jobs);
        }
    }
}