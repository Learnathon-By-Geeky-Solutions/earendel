using MediatR;

namespace TalentMesh.Module.InterviewerView
{
    // still IRequest<Unit> (i.e. IRequest) 
    public record RejectInterviewerCommand(Guid EntryFormId) : IRequest<Unit>;
}
