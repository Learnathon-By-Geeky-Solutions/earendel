using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Update.v1;

public sealed record UpdateQuizQuestionCommand(
    Guid Id,
    string QuestionText,
    int CorrectOption
) : IRequest<UpdateQuizQuestionResponse>;
