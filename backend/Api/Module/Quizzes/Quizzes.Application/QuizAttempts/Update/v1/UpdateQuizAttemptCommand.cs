using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Update.v1;
public sealed record UpdateQuizAttemptCommand(
    Guid Id,
    Guid UserId,
    decimal Score,
    int TotalQuestions) : IRequest<UpdateQuizAttemptResponse>;
