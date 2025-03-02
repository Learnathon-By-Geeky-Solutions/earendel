using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Delete.v1;
public sealed record DeleteQuizQuestionCommand(
    Guid Id) : IRequest;
