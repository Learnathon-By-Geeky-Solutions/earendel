using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Framework.Infrastructure.Messaging;

namespace TalentMesh.Module.Job.Application.JobApplication.Create.v1;
public sealed class CreateJobApplicationHandler(
    ILogger<CreateJobApplicationHandler> logger,
    [FromKeyedServices("jobs:jobapplication")] IRepository<Domain.JobApplication> repository,
    IMessageBus messageBus
    )
    : IRequestHandler<CreateJobApplicationCommand, CreateJobApplicationResponse>
{
    public async Task<CreateJobApplicationResponse> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var jobApplication = Domain.JobApplication.Create(
            request.JobId!,
            request.CandidateId!,
            request.CoverLetter!
        );
        await repository.AddAsync(jobApplication, cancellationToken);

        var notificationMessage = new
        {
            UserId = "null",
            Entity = request.JobId, // Send the JobId so that Consumer can find the Job Poster
            EntityType = "JobApplication", // You can set "JobApplication" as entity type
            Message = "A new candidate has applied for your job posting.", // Your notification text
            RequestedBy = request.JobId
        };

        await messageBus.PublishAsync(notificationMessage, "notification.events", "notification.fetched", cancellationToken);

        logger.LogInformation("JobApplication Created {UserId}", jobApplication.Id);
        return new CreateJobApplicationResponse(jobApplication.Id);
    }
}
