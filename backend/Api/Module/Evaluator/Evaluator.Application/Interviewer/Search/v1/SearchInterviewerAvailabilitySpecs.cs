using Ardalis.Specification;
using Evaluator.Application.Interviewer.Get.v1;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1
{
    public class SearchInterviewerAvailabilitySpecs : EntitiesByPaginationFilterSpec<InterviewerAvailability, InterviewerAvailabilityResponse>
    {
        public SearchInterviewerAvailabilitySpecs(SearchInterviewerAvailabilitiesCommand command)
            : base(command)
        {
            // Order by StartTime if no specific order is provided.
            Query.OrderBy(x => x.StartTime, !command.HasOrderBy());

            // Filter by InterviewerId if provided.
            if (command.InterviewerId.HasValue)
            {
                Query.Where(x => x.InterviewerId == command.InterviewerId.Value);
            }

            // Filter by IsAvailable if provided.
            if (command.IsAvailable.HasValue)
            {
                Query.Where(x => x.IsAvailable == command.IsAvailable.Value);
            }
        }
    }
}
