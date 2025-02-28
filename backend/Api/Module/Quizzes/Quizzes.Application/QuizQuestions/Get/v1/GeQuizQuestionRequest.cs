using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Get.v1;
public class GetQuizQuestionRequest : IRequest<QuizQuestionResponse>
{
    public Guid Id { get; set; }
    public GetQuizQuestionRequest(Guid id) => Id = id;
}
