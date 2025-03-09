using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using TalentMesh.Module.Experties.Domain;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Search.v1;
public class SearchSeniorityLevelJunctionSpecs : EntitiesByPaginationFilterSpec<Experties.Domain.SeniorityLevelJunction, SeniorityLevelJunctionResponse>
{
    public SearchSeniorityLevelJunctionSpecs(SearchSeniorityLevelJunctionCommand command)
        : base(command) =>
        Query
            .Where(x => x.SeniorityLevelId == command.SeniorityLevelId, command.SeniorityLevelId.HasValue)
            .Where(x => x.SkillId == command.SkillId, command.SkillId.HasValue)
            .OrderBy(x => x.SeniorityLevelId);
}