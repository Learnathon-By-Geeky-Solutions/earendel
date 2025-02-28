using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Update.v1;

public sealed record UpdateQuizQuestionCommand(
    Guid Id,
    string QuestionText,
    string Option1,
    string Option2,
    string Option3,
    string Option4,
    int CorrectOption
) : IRequest<UpdateQuizQuestionResponse>;
