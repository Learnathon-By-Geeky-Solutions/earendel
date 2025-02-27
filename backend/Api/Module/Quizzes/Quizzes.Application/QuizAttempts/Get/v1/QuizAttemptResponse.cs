namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Get.v1;
public sealed record QuizAttemptResponse(Guid? Id, Guid? UserId,  decimal Score, int TotalQuestions);
