using MediatR;

namespace TalentMesh.Module.Evaluator.Application.Interviewer
{
    public record RequestInterviewerCommand(Guid UserId, string? AdditionalInfo) : IRequest<Guid>;
}
