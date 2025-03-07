using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Get.v1;
using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Search.v1;

public class SearchInterviewQuestionsCommand : PaginationFilter, IRequest<PagedList<InterviewQuestionResponse>>
{
    public Guid? RubricId { get; set; }
    public string? QuestionText { get; set; }  
}
