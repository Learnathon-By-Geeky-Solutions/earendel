using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Job.Application.JobApplication.Create.v1
{
    public sealed record CreateJobApplicationCommand(
        Guid JobId,
        Guid CandidateId,
        string? CoverLetter = null
    ) : IRequest<CreateJobApplicationResponse>;
}


