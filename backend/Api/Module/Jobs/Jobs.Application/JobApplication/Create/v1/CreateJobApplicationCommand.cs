using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Job.Application.JobApplication.Create.v1
{
    public sealed record CreateJobApplicationCommand(
        int JobId,
        int CandidateId,
        string? CoverLetter = "Sample CoverLetter"
    ) : IRequest<CreateJobApplicationResponse>;
}


