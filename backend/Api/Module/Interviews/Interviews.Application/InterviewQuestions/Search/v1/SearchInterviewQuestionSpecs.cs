using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Get.v1;
using TalentMesh.Module.Interviews.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchInterviewQuestionSpecs : EntitiesByPaginationFilterSpec<InterviewQuestion, InterviewQuestionResponse>
{
    public SearchInterviewQuestionSpecs(SearchInterviewQuestionsCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.QuestionText, !command.HasOrderBy());
}
