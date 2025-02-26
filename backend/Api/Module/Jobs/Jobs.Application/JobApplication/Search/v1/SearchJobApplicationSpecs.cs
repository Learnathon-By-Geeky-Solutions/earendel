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

            // If a specific JobId filter is provided, apply an exact match
            if (command.JobId.HasValue)
            {
                Query.Where(b => b.JobId == command.JobId.Value);
            }

            // If a keyword is provided, search by CandidateId or JobId (as strings)
            if (!string.IsNullOrEmpty(command.Keyword))
            {
                Query.Where(b => b.CandidateId.ToString().Contains(command.Keyword)
                                 || b.JobId.ToString().Contains(command.Keyword));
            }
        }
    }
}
