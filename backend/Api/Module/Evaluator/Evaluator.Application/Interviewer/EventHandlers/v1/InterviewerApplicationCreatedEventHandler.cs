using MediatR;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Evaluator.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace Evaluator.Application.Interviewer.EventHandlers.v1
{
    [ExcludeFromCodeCoverage]
    public class InterviewerApplicationCreatedEventHandler(ILogger<InterviewerApplicationCreatedEventHandler> logger)
        : INotificationHandler<InterviewerApplicationCreated>
    {
        public async Task Handle(InterviewerApplicationCreated notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling InterviewerApplicationCreated domain event...");
            await Task.CompletedTask; // or any asynchronous processing
            logger.LogInformation("Finished handling InterviewerApplicationCreated domain event.");
        }
    }
}
