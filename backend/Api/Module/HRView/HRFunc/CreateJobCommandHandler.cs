using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Infrastructure.Identity.Users.Services;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Framework.Infrastructure.SignalR;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Infrastructure.Persistence; // For IResult and Results

namespace TalentMesh.Module.HRView.HRFunc // Or your preferred namespace
{
    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, IResult>
    {
        private readonly JobDbContext _context;
        private readonly IMessageBus _messageBus;
        private readonly IExternalApiClient _apiClient;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CreateJobCommandHandler(JobDbContext context, IMessageBus messageBus, IExternalApiClient apiClient, IHubContext<NotificationHub> hubContext)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));

            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
        }

        public async Task<IResult> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            // 1. Create the Job entity
            var jobInfo = new JobInfo
            {
                Name = request.Name,
                Description = request.Description,
                Requirments = request.Requirments,
                Location = request.Location,
                JobType = request.JobType,
                ExperienceLevel = request.ExperienceLevel,
                Salary = request.Salary ?? string.Empty,
                PostedById = request.PostedBy
            };

            var newJob = Jobs.Create(jobInfo);
            // 2. Add the Job to the context BUT DON'T SAVE YET if generating ID client-side
            // If using database-generated IDs, you MUST save here to get the ID
            // Assuming database-generated Guid ID for Job:
            _context.Jobs.Add(newJob);
            await _context.SaveChangesAsync(cancellationToken); // Save to get the newJob.Id

            // 3. Create and add JobRequiredSkill entities
            if (request.RequiredSkillIds != null && request.RequiredSkillIds.Count > 0)
            {
                var skillsToAdd = request.RequiredSkillIds
                    .Distinct() // Avoid duplicates
                    .Select(skillId => JobRequiredSkill.Create(newJob.Id, skillId))
                    .ToList();
                await _context.JobRequiredSkill.AddRangeAsync(skillsToAdd, cancellationToken);
            }

            // 4. Create and add JobRequiredSubskill entities
            if (request.RequiredSubskillIds != null && request.RequiredSubskillIds.Count > 0)
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
            string gatewayPageURL = await _apiClient.InitiateSslCommerzPaymentAsync(newJob.Id.ToString(), request.Salary!, cancellationToken);

            var notificationMessage = new
            {
                UserId = "34a12d68-9d87-4ef6-95fe-53bd6f5d3b97",
                Entity = "Job",
                EntityType = "Job",
                Message = $"{request.PostedBy} post a new job",
                RequestedBy = newJob.Id
            };

            await _messageBus.PublishAsync(notificationMessage, "notification.events", "notification.fetched", cancellationToken);

            await _hubContext.Clients.Group("admin").SendAsync("SystemAlert", "A new job post is created by a HR", cancellationToken);

            // 6. Return the ID of the newly created job
            // Consider returning StatusCodes.Status201Created with the location header
            return Results.Created($"/jobs/{newJob.Id}", new { JobId = newJob.Id, Payment = gatewayPageURL });
        }
    }
}
