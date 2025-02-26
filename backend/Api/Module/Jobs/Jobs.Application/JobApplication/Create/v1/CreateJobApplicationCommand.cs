using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Job.Application.JobApplication.Create.v1
{
    public sealed record CreateJobApplicationCommand(
        Guid JobId,
        Guid CandidateId,
        string? CoverLetter = "Provide a CoverLetter MAX Length 500"
    ) : IRequest<CreateJobApplicationResponse>;
}


