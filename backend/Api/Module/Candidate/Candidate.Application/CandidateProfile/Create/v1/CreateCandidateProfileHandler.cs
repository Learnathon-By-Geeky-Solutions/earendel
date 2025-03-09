using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Candidate.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Framework.Core.Exceptions;
using System.Security.Claims;


namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Create.v1;

public sealed class CreateCandidateProfileHandler(
    ILogger<CreateCandidateProfileHandler> logger,
    [FromKeyedServices("candidate:candidateprofile")] IRepository<Domain.CandidateProfile> repository)
    : IRequestHandler<CreateCandidateProfileCommand, CreateCandidateProfileResponse>
{
    public async Task<CreateCandidateProfileResponse> Handle(CreateCandidateProfileCommand request, CancellationToken cancellationToken)
    {
        //ArgumentNullException.ThrowIfNull(request);
        //if (user.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        //{
        //    throw new UnauthorizedException();
        //}
        var candidateProfile = Domain.CandidateProfile.Create(
            request.UserId!,
            request.Resume!,
            request.Skills!,
            request.Experience!,
            request.Education!
        );
        await repository.AddAsync(candidateProfile, cancellationToken);
        logger.LogInformation("CandidateProfile Created {UserId}", candidateProfile.Id);
        return new CreateCandidateProfileResponse(candidateProfile.Id);
    }
}


