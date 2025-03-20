using MediatR;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Get.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchCandidateProfileCommand : PaginationFilter, IRequest <PagedList<CandidateProfileResponse>>
{
    public Guid? Id { get; set; }
    public string? Resume { get; set; }
    public string? Skills { get; set; }
    public string? Experience { get; set; }
    public string? Education { get; set; }
    public Guid? UserId { get; set; }

}

