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


            var postedJobIds = _context.Jobs
            .AsNoTracking()
            .Where(j => j.PostedById == request.RequestingUserId)
            .Select(j => j.Id);

            // 2) filter applications by JobId FK
            var query = _context.JobApplications
                .AsNoTracking()
                .Where(app => postedJobIds.Contains(app.JobId))
                .OrderByDescending(app => app.ApplicationDate);


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
                    CreatedOn = app.Created,
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