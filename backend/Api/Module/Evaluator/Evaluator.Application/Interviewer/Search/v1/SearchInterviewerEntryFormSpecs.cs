using Ardalis.Specification;
using Evaluator.Application.Interviewer.Get.v1;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1;
using TalentMesh.Module.Evaluator.Domain;


namespace TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1
{
    public class SearchInterviewerEntryFormSpecs : EntitiesByPaginationFilterSpec<InterviewerEntryForm, InterviewerEntryFormResponse>
    {
        public SearchInterviewerEntryFormSpecs(SearchInterviewerEntryFormsCommand command)
            : base(command)
        {
            // Order by CreatedDate (assumed from AuditableEntity) if no specific order is provided.
            Query.OrderBy(x => x.Created, !command.HasOrderBy())
                 // Filter by AdditionalInfo if provided.
                 .Where(x => !string.IsNullOrEmpty(command.AdditionalInfo) ? x.AdditionalInfo.Contains(command.AdditionalInfo) : true)
                 // Filter by Status if provided.
                 .Where(x => !string.IsNullOrEmpty(command.Status) ? x.Status.Contains(command.Status) : true);
        }
    }
}
