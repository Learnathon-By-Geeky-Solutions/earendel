using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Create.v1;

public sealed record CreateInterviewFeedbackCommand(
    Guid InterviewId,
    Guid InterviewQuestionId,
    string Response,
    decimal Score
) : IRequest<CreateInterviewFeedbackResponse>;
