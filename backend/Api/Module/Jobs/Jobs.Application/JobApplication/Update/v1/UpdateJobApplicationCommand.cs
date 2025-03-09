using MediatR;

namespace TalentMesh.Module.Job.Application.JobApplication.Update.v1
{
    public sealed record UpdateJobApplicationCommand(
        Guid Id,
        Guid JobId,
        Guid CandidateId,
        string Status,
        string? CoverLetter = null
    ) : IRequest<UpdateJobApplicationResponse>;
}
