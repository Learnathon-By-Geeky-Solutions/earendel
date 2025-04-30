using TalentMesh.Module.Job.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.EventHandlers
{
    [ExcludeFromCodeCoverage]
    public class JobRequiredSkillCreatedEventHandler : INotificationHandler<JobRequiredSkillCreated>
    {
        private readonly ILogger<JobRequiredSkillCreatedEventHandler> _logger;

        public JobRequiredSkillCreatedEventHandler(ILogger<JobRequiredSkillCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(JobRequiredSkillCreated notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling JobRequiredSkillCreated domain event...");
            await Task.CompletedTask; // Replace with your asynchronous logic if needed.
            _logger.LogInformation("Finished handling JobRequiredSkillCreated domain event.");
        }
    }
}
