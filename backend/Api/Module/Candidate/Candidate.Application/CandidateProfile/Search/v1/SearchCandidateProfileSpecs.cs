using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Get.v1;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Search.v1;

public class SearchCandidateProfileSpecs : EntitiesByPaginationFilterSpec<Domain.CandidateProfile, CandidateProfileResponse>
{
    public SearchCandidateProfileSpecs(SearchCandidateProfileCommand command)
        : base(command)
    {
        Query.OrderBy(c => c.UserId, !command.HasOrderBy());

        if (command.UserId != null)
        {
            Query.Where(b => b.UserId == command.UserId.Value);
        }

        if (command.Id.HasValue)
        {
            // Compare just the date portion if needed.
            Query.Where(b => b.UserId == command.Id.Value);
        }

        if (!string.IsNullOrEmpty(command.Skills))
        {
            Query.Where(b => b.Skills.Contains(command.Skills));
        }

        
    }
}
