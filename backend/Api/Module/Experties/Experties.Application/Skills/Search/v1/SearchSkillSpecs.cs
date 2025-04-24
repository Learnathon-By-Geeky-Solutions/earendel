using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Experties.Application.Skills.Get.v1;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using TalentMesh.Module.Experties.Domain;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TalentMesh.Module.Experties.Application.Skills.Search.v1
{
    [ExcludeFromCodeCoverage]
    public class SearchSkillSpecs : Specification<Skill, SkillResponse>
    {
        public SearchSkillSpecs(SearchSkillsCommand command)
        {
            // Filter: non-deleted and optional keyword
            Query.Where(skill =>
                skill.DeletedBy == null &&
                (string.IsNullOrEmpty(command.Keyword) || skill.Name.Contains(command.Keyword))
            );

            // Apply default ordering flag directly
            Query.OrderBy(skill => skill.Name, !command.HasOrderBy());

            Query.AsSplitQuery();

            // Projection remains unchanged
            Query.Select(skill => new SkillResponse(
                skill.Id,
                skill.Name,
                skill.Description,
                skill.SubSkills
                    .Where(sub => sub.DeletedBy == null)
                    .Select(sub => new SubSkillResponse(
                        sub.Id,
                        sub.Name,
                        sub.Description,
                        sub.SkillId
                    )).ToList(),
                skill.SeniorityLevelJunctions
                    .Where(j => j.DeletedBy == null && j.Seniority.DeletedBy == null)
                    .Select(j => new SeniorityLevelJunctionResponse(
                        j.Id,
                        j.Seniority.Id,
                        j.SkillId,
                        new SeniorityResponse(
                            j.Seniority.Id,
                            j.Seniority.Name,
                            j.Seniority.Description ?? string.Empty
                        )
                    )).ToList()
            ));
        }
    }
}
