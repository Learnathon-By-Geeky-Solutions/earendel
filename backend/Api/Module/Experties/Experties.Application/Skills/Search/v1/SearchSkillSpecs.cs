using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Experties.Application.Skills.Get.v1;
using TalentMesh.Module.Experties.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Skills.Search.v1
{
    [ExcludeFromCodeCoverage]
    public class SearchSkillSpecs : Specification<Skill, SkillResponse>
    {
        public SearchSkillSpecs(SearchSkillsCommand command)
        {
            // Filter by keyword on skill name if provided.
            if (!string.IsNullOrEmpty(command.Keyword))
            {
                Query.Where(b => b.Name.Contains(command.Keyword));
            }

            // Include related SubSkills.
            Query.Include(x => x.SubSkills);

            Query.Include(x => x.SeniorityLevelJunctions)
                 .ThenInclude(j => j.Seniority);

            // Apply ordering if no explicit order is provided.
            if (!command.HasOrderBy())
            {
                Query.OrderBy(c => c.Name);
            }
            Query.AsSplitQuery();

        }
    }
}
