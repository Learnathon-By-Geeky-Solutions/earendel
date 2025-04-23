using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Get.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchCandidateProfileSpecs : EntitiesByPaginationFilterSpec<Domain.CandidateProfile, CandidateProfileResponse>
{
    public SearchCandidateProfileSpecs(SearchCandidateProfileCommand command)
        : base(command)
    {
        // Combine pagination, ordering, and all filters without branching
        Query
            .Where(c =>
                // Filter by UserId if provided
                (command.UserId == null || c.UserId == command.UserId) &&

                // Filter by Id if provided
                (!command.Id.HasValue || c.Id == command.Id.Value) &&

                // Filter by Skills substring safely (guarding null)
                (string.IsNullOrEmpty(command.Skills)
                    || (c.Skills != null && c.Skills.Contains(command.Skills)))
            )
            .OrderBy(c => c.UserId, !command.HasOrderBy());


    }
}
