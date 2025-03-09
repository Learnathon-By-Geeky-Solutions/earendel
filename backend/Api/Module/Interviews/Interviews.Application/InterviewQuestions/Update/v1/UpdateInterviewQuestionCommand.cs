using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Update.v1;

public sealed record UpdateInterviewQuestionCommand(
    Guid Id,
    Guid RubricId,
    Guid InterviewId,
    string? QuestionText
) : IRequest<UpdateInterviewQuestionResponse>;
