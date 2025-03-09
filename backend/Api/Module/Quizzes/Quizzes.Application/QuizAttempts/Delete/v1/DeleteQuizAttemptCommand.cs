using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Delete.v1;
public sealed record DeleteQuizAttemptCommand(
    Guid Id) : IRequest;
