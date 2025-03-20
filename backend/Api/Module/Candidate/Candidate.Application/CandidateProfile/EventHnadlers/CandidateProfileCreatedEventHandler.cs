using MediatR;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Candidate.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.EventHnadlers;

[ExcludeFromCodeCoverage]

class CandidateProfileCreatedEventHandler(ILogger<CandidateProfileCreatedEventHandler> logger) : INotificationHandler<CandidateProfileCreated>
{
    public async Task Handle(CandidateProfileCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling CandidateProfile created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling CandidateProfile created domain event..");
    }
}


