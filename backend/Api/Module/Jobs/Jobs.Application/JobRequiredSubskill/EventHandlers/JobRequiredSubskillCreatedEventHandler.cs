using TalentMesh.Module.Job.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.EventHandlers
{
    public class JobRequiredSubskillCreatedEventHandler : INotificationHandler<JobRequiredSubskillCreated>
    {
        private readonly ILogger<JobRequiredSubskillCreatedEventHandler> _logger;

        public JobRequiredSubskillCreatedEventHandler(ILogger<JobRequiredSubskillCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(JobRequiredSubskillCreated notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling JobRequiredSubskillCreated domain event...");
            await Task.CompletedTask; // Replace with your asynchronous logic if needed.
            _logger.LogInformation("Finished handling JobRequiredSubskillCreated domain event.");
        }
    }
}
