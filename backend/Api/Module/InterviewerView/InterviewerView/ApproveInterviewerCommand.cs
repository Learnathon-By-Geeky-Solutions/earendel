using MediatR;

namespace TalentMesh.Module.Evaluator.Application.Interviewer
{
    // still IRequest<Unit> (i.e. IRequest) 
    public record ApproveInterviewerCommand(Guid EntryFormId) : IRequest<Unit>;
}
