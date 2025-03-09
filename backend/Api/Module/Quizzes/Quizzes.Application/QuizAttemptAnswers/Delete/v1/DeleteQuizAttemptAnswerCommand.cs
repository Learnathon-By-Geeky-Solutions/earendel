using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Delete.v1;
public sealed record DeleteQuizAttemptAnswerCommand(
    Guid Id) : IRequest;
