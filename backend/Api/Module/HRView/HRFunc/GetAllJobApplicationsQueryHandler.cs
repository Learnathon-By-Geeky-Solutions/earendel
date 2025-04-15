using MediatR;
using Microsoft.AspNetCore.Http; // For IResult and Results
using Microsoft.EntityFrameworkCore; // For EF Core methods like Include, Skip, Take
using TalentMesh.Module.Job.Infrastructure.Persistence; // Your DbContext namespace

namespace TalentMesh.Module.HRView.HRFunc // Or your preferred namespace
{
    public class GetAllJobApplicationsQueryHandler : IRequestHandler<GetAllJobApplicationsQuery, IResult>
    {
        private readonly JobDbContext _context;

        public GetAllJobApplicationsQueryHandler(JobDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(GetAllJobApplicationsQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
            int pageSize = request.PageSize > 0 ? request.PageSize : 20;

            var query = _context.JobApplications
                .AsNoTracking()
                .Include(app => app.Job) // Essential for filtering by Job.PostedById
                                         // *** Add filter for jobs posted by the requesting user ***
                .Where(app => app.Job != null && app.Job.PostedById == request.RequestingUserId) // Assuming Jobs.PostedById exists
                .OrderByDescending(app => app.ApplicationDate); // Example ordering

            var totalCount = await query.CountAsync(cancellationToken);

            var applications = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(app => new JobApplicationDto
                {
                    Id = app.Id,
                    JobId = app.JobId,
                    CandidateId = app.CandidateId,
                    ApplicationDate = app.ApplicationDate,
                    Status = app.Status,
                    CoverLetter = app.CoverLetter,
                    CreatedOn = app.CreatedOn,
                    JobName = app.Job != null ? app.Job.Name : null
                })
                .ToListAsync(cancellationToken);

            // Optional: Return structured paginated result
            // var paginatedResult = new PaginatedResult<JobApplicationDto>(applications, totalCount, pageNumber, pageSize);
            // return Results.Ok(paginatedResult);

            return Results.Ok(applications);
        }
    }
}