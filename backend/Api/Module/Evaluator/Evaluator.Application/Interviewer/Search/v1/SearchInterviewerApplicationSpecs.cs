using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using System;
using Evaluator.Application.Interviewer.Get.v1;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1
{
    public class SearchInterviewerApplicationSpecs : EntitiesByPaginationFilterSpec<InterviewerApplication, InterviewerApplicationResponse>
    {
        public SearchInterviewerApplicationSpecs(SearchInterviewerApplicationsCommand command)
            : base(command)
        {
            // Order by AppliedDate (ascending by default) if no specific order is provided.
            Query.OrderBy(x => x.AppliedDate, !command.HasOrderBy());

            // Filter by Status if provided.
            if (!string.IsNullOrEmpty(command.Status))
            {
                Query.Where(x => x.Status.Contains(command.Status, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by Comments if provided.
            if (!string.IsNullOrEmpty(command.Comments))
            {
                Query.Where(x => x.Comments != null && x.Comments.Contains(command.Comments, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
