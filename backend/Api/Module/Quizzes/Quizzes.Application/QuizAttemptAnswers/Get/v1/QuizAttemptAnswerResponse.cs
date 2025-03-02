namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Get.v1;

public sealed record QuizAttemptAnswerResponse(
    Guid Id,
    Guid AttemptId,
    Guid QuestionId,
    int SelectedOption,
    bool IsCorrect
);