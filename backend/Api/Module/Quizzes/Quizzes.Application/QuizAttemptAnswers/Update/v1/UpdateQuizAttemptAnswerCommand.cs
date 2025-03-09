using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Update.v1;

public sealed record UpdateQuizAttemptAnswerCommand(
    Guid Id,
    Guid AttemptId,
    Guid QuestionId,
    int SelectedOption,
    bool IsCorrect
) : IRequest<UpdateQuizAttemptAnswerResponse>;