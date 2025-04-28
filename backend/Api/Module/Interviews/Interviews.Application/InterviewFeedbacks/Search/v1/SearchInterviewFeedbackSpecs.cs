using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Get.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Search.v1;
[ExcludeFromCodeCoverage]
public class SearchInterviewFeedbackSpecs : EntitiesByPaginationFilterSpec<InterviewFeedback, InterviewFeedbackResponse>
{
    public SearchInterviewFeedbackSpecs(SearchInterviewFeedbacksCommand command)
        : base(command)
    {
        if (command.InterviewId.HasValue)
            Query.Where(c => c.InterviewId == command.InterviewId.Value);

        if (!string.IsNullOrEmpty(command.InterviewQuestionText))
            Query.Where(c => c.Response.Contains(command.InterviewQuestionText));

        if (!string.IsNullOrEmpty(command.Response))
            Query.Where(c => c.Response.Contains(command.Response));

        if (command.Score.HasValue)
            Query.Where(c => c.Score >= command.Score.Value);

        // Default sorting by InterviewId (ascending) unless another order is specified
        Query.OrderBy(c => c.InterviewId, !command.HasOrderBy());
    }
}
