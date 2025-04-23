using MediatR;
using Microsoft.AspNetCore.Http;
using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Infrastructure.Identity.Users.Services;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Infrastructure.Persistence; // For IResult and Results

namespace TalentMesh.Module.HRView.HRFunc // Or your preferred namespace
{
    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, IResult>
    {
        private readonly JobDbContext _context;
        private readonly IExternalApiClient apiClient;

        public CreateJobCommandHandler(JobDbContext context, IdentityServices identityServices)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            apiClient = identityServices.ApiClient ?? throw new ArgumentNullException(nameof(identityServices.ApiClient));
        }

        public async Task<IResult> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            // 1. Create the Job entity
            var newJob = Jobs.Create(
                name: request.Name,
                description: request.Description,
                requirments: request.Requirments, // Correct spelling if needed
                location: request.Location,
                jobType: request.JobType,
                experienceLevel: request.ExperienceLevel,
                salary: request.Salary ?? string.Empty, // Provide default if null
                postedById: request.PostedBy // Assuming this is a Guid
            );

            // 2. Add the Job to the context BUT DON'T SAVE YET if generating ID client-side
            // If using database-generated IDs, you MUST save here to get the ID
            // Assuming database-generated Guid ID for Job:
            _context.Jobs.Add(newJob);
            await _context.SaveChangesAsync(cancellationToken); // Save to get the newJob.Id

            // 3. Create and add JobRequiredSkill entities
            if (request.RequiredSkillIds != null && request.RequiredSkillIds.Any())
            {
                var skillsToAdd = request.RequiredSkillIds
                    .Distinct() // Avoid duplicates
                    .Select(skillId => JobRequiredSkill.Create(newJob.Id, skillId))
                    .ToList();
                await _context.JobRequiredSkill.AddRangeAsync(skillsToAdd, cancellationToken);
            }

            // 4. Create and add JobRequiredSubskill entities
            if (request.RequiredSubskillIds != null && request.RequiredSubskillIds.Any())
            {
                var subskillsToAdd = request.RequiredSubskillIds
                    .Distinct() // Avoid duplicates
                    .Select(subskillId => JobRequiredSubskill.Create(newJob.Id, subskillId))
                    .ToList();
                await _context.JobRequiredSubskill.AddRangeAsync(subskillsToAdd, cancellationToken);
            }

            // 5. Save the associated skills and subskills (if not saved in step 2)
            // If Job was saved in step 2, this saves the Skills/Subskills
            await _context.SaveChangesAsync(cancellationToken);

            // Job ID For SSLCommerz Payment, amount = Number of Interviews * 1000
            string gatewayPageURL = await apiClient.InitiateSslCommerzPaymentAsync(newJob.Id.ToString(), request.Salary, cancellationToken);

            // 6. Return the ID of the newly created job
            // Consider returning StatusCodes.Status201Created with the location header
            return Results.Created($"/jobs/{newJob.Id}", new { JobId = newJob.Id, Payment = gatewayPageURL });
        }
    }
}
