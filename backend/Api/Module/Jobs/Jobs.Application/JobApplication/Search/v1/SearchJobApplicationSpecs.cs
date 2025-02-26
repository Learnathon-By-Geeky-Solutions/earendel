using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Job.Application.JobApplication.Get.v1;
using TalentMesh.Module.Job.Domain;

namespace TalentMesh.Module.Job.Application.JobApplication.Search.v1
{
    public class SearchJobApplicationSpecs : EntitiesByPaginationFilterSpec<Domain.JobApplication, JobApplicationResponse>
    {
        public SearchJobApplicationSpecs(SearchJobApplicationsCommand command)
            : base(command)
        {
            Query.OrderBy(c => c.ApplicationDate, !command.HasOrderBy());

            if (command.CandidateId.HasValue)
            {
                Query.Where(b => b.CandidateId == command.CandidateId.Value);
            }

            if (command.ApplicationDate.HasValue)
            {
                // Compare just the date portion if needed.
                Query.Where(b => b.ApplicationDate.Date == command.ApplicationDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(command.Status))
            {
                Query.Where(b => b.Status.Contains(command.Status));
            }

            if (command.JobId.HasValue)
            {
                Query.Where(b => b.JobId == command.JobId.Value);
            }
        }
    }
}
