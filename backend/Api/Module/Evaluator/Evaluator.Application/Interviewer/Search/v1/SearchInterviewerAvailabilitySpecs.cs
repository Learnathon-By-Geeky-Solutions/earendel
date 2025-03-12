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
            Query.OrderBy(x => x.StartTime, !command.HasOrderBy())
                 // Filter by InterviewerId if provided.
                 .Where(x => command.InterviewerId.HasValue ? x.InterviewerId == command.InterviewerId.Value : true)
                 // Filter by IsAvailable if provided.
                 .Where(x => command.IsAvailable.HasValue ? x.IsAvailable == command.IsAvailable.Value : true);
        }
    }
}
