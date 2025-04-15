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
            // Basic validation for pagination parameters
            int pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
            int pageSize = request.PageSize > 0 ? request.PageSize : 20; // Set a max limit if desired

            var query = _context.JobApplications
                              .AsNoTracking()
                              // Include Job details if JobName is needed in DTO
                              .Include(app => app.Job)
                              .OrderByDescending(app => app.ApplicationDate); // Example ordering

            // Get total count for pagination metadata (optional but good practice)
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var applications = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(app => new JobApplicationDto // Map to DTO
                {
                    Id = app.Id,
                    JobId = app.JobId,
                    CandidateId = app.CandidateId,
                    ApplicationDate = app.ApplicationDate,
                    Status = app.Status,
                    CoverLetter = app.CoverLetter, // Include if needed
                    CreatedOn = app.Created,
                    JobName = app.Job != null ? app.Job.Name : null // Safely access included Job Name
                })
                .ToListAsync(cancellationToken);

            // paginated response object:
            // var paginatedResult = new PaginatedResult<JobApplicationDto>(applications, totalCount, pageNumber, pageSize);
            // return Results.Ok(paginatedResult);

            // For simplicity, returning the list directly:
            return Results.Ok(applications);
        }
    }
}