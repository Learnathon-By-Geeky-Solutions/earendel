using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Candidate.Domain.Exceptions;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Update.v1
{
    public sealed class UpdateCandidateProfileHandler(
        ILogger<UpdateCandidateProfileHandler> logger,
        [FromKeyedServices("candidate:candidateprofile")] IRepository<Domain.CandidateProfile> repository)
        : IRequestHandler<UpdateCandidateProfileCommand, UpdateCandidateProfileResponse>
    {
        public async Task<UpdateCandidateProfileResponse> Handle(UpdateCandidateProfileCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var candidateProfile = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (candidateProfile == null) throw new CandidateProfileNotFoundException(request.Id);


            var updatedJobApplication = candidateProfile.Update(request.Resume, request.Skills, request.Experience, request.Education);
            await repository.UpdateAsync(updatedJobApplication, cancellationToken);

            logger.LogInformation("Job application with id : {JobApplicationId} updated.", candidateProfile.Id);
            return new UpdateCandidateProfileResponse(candidateProfile.Id, candidateProfile.Resume, candidateProfile.Skills, candidateProfile.Experience, candidateProfile.Education);
        }
    }
}
