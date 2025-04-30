using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Get.v1;

[ExcludeFromCodeCoverage]
public sealed record QuizQuestionResponse(
    Guid? Id,
    string QuestionText,
    string Option1,
    string Option2,
    string Option3,
    string Option4,
    int CorrectOption
);
