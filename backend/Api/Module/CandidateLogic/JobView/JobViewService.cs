using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Module.Job.Infrastructure.Persistence; // Your DbContext namespace

namespace TalentMesh.Module.CandidateLogic.JobView // Or your preferred namespace
{
    [ExcludeFromCodeCoverage]
    public class JobViewService : IRequestHandler<JobViewFilters, IResult>
    {
        private readonly JobDbContext _jobDbContext; // Renamed for clarity

        public JobViewService(JobDbContext context)
        {
            _jobDbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(JobViewFilters request, CancellationToken cancellationToken)
        {
            // --- 1. Filter Jobs ---
            var jobQuery = _jobDbContext.Jobs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                // Normalize both database value and filter
                var normalizedFilter = request.Name
                    .Trim()
                    .Replace(" ", "")
                    .ToLower(); // Case normalization

                jobQuery = jobQuery.Where(j =>
                    EF.Functions.Like(
                        j.Name.Replace(" ", "").ToLower(), // Database-side normalization
                        $"%{normalizedFilter}%"
                    )
                );
            }


            if (!string.IsNullOrWhiteSpace(request.Description))
                jobQuery = jobQuery.Where(j => j.Description != null && j.Description.Contains(request.Description));

            if (!string.IsNullOrWhiteSpace(request.Requirements))
                jobQuery = jobQuery.Where(j => j.Requirments.Contains(request.Requirements));

            if (!string.IsNullOrWhiteSpace(request.Location))
                jobQuery = jobQuery.Where(j => j.Location.Contains(request.Location));

            if (!string.IsNullOrWhiteSpace(request.JobType))
                jobQuery = jobQuery.Where(j => j.JobType.Contains(request.JobType));

            if (!string.IsNullOrWhiteSpace(request.ExperienceLevel))
                jobQuery = jobQuery.Where(j => j.ExperienceLevel.Contains(request.ExperienceLevel));


            var filteredJobs = await jobQuery
                .Select(j => new // Project to anonymous type initially to reduce data transfer
                {
                    j.Id,
                    j.Name,
                    j.Description,
                    j.Requirments,
                    j.Location,
                    j.JobType,
                    j.ExperienceLevel,
                    j.Created,
                    j.Salary,
                    j.PostedById,
                    j.PaymentStatus,
                })
                .ToListAsync(cancellationToken);

            if (filteredJobs.Count == 0)
            {
                return Results.Ok(new List<JobViewDto>()); // Return empty list if no jobs found
            }

            var jobIds = filteredJobs.Select(j => j.Id).ToList();

            // --- 2. Fetch Required Skills & Subskills ---
            var requiredSkills = await _jobDbContext.JobRequiredSkill
                .AsNoTracking()
                .Where(rs => jobIds.Contains(rs.JobId))
                .Select(rs => new { rs.JobId, rs.SkillId }) // Select only needed data
                .ToListAsync(cancellationToken);

            var requiredSubskills = await _jobDbContext.JobRequiredSubskill
                .AsNoTracking()
                .Where(rss => jobIds.Contains(rss.JobId))
                .Select(rss => new { rss.JobId, rss.SubskillId }) // Select only needed data
                .ToListAsync(cancellationToken);

            // --- 3. Group Skills/Subskills by JobId ---
            var skillsByJobId = requiredSkills.ToLookup(rs => rs.JobId, rs => rs.SkillId);
            var subskillsByJobId = requiredSubskills.ToLookup(rss => rss.JobId, rss => rss.SubskillId);

            // --- 4. Map to DTO ---
            var results = filteredJobs.Select(job => new JobViewDto
            {
                Id = job.Id,
                Name = job.Name,
                Description = job.Description,
                Requirments = job.Requirments,
                Location = job.Location,
                JobType = job.JobType,
                ExperienceLevel = job.ExperienceLevel,
                CreatedOn = job.Created,
                PaymentStatus = job.PaymentStatus,
                Salary = job.Salary ?? "",
                PostedById = job.PostedById,    
                RequiredSkillIds = skillsByJobId.Contains(job.Id) ? skillsByJobId[job.Id].ToList() : new List<Guid>(),
                RequiredSubskillIds = subskillsByJobId.Contains(job.Id) ? subskillsByJobId[job.Id].ToList() : new List<Guid>()
            }).ToList();

            return Results.Ok(results);
        }
    }
}