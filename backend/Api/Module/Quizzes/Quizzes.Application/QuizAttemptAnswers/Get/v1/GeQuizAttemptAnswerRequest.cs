using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Get.v1;
public class GetQuizAttemptAnswerRequest : IRequest<QuizAttemptAnswerResponse>
{
    public Guid Id { get; set; }
    public GetQuizAttemptAnswerRequest(Guid id) => Id = id;
}
