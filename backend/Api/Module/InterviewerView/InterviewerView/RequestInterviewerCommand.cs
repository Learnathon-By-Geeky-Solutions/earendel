using System.Diagnostics.CodeAnalysis;
using MediatR;
using TalentMesh.Module.Evaluator.Application.Interviewer;

namespace TalentMesh.Module.InterviewerView;
[ExcludeFromCodeCoverage]
public class RequestInterviewerCommand : IRequest<RequestInterviewerResult>
{
    public Guid UserId { get; }
    public string? AdditionalInfo { get; }

    public RequestInterviewerCommand(Guid userId, string? additionalInfo)
    {
        UserId = userId;
        AdditionalInfo = additionalInfo;
    }
}