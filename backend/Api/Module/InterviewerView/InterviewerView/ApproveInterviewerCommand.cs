using MediatR;

namespace TalentMesh.Module.InterviewerView
{
    // still IRequest<Unit> (i.e. IRequest) 
    public record ApproveInterviewerCommand(Guid EntryFormId) : IRequest<Unit>;
}
