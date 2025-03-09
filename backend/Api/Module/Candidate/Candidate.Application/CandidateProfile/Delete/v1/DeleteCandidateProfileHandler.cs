using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Candidate.Domain.Exceptions;
using TalentMesh.Module.Candidate.Domain.Extentsion;


namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Delete.v1;

public sealed class DeleteCandidateProfileHandler(
    ILogger<DeleteCandidateProfileHandler> logger,
    [FromKeyedServices("candidate:candidateprofile")] IRepository<Domain.CandidateProfile> repository)
    : IRequestHandler<DeleteCandidateProfileCommand>
{
    public async Task Handle(DeleteCandidateProfileCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var candidateProfile = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (candidateProfile.IsDeletedOrNotFound()) throw new CandidateProfileNotFoundException(request.Id);
        await repository.DeleteAsync(candidateProfile, cancellationToken);
        logger.LogInformation("CandidateProfile with id : {Id} deleted", candidateProfile.Id);
    }
}

