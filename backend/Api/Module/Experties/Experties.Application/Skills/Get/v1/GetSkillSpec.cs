using Ardalis.Specification;
using TalentMesh.Module.Experties.Domain;

namespace TalentMesh.Module.Experties.Application.Skills.Get.v1
{
    public class GetSkillSpec : Specification<Skill>, ISingleResultSpecification<Skill>
    {
        public GetSkillSpec(Guid id)
        {
            Query.Where(s => s.Id == id)
                 .Include(s => s.SubSkills)
                 .Include(s => s.SeniorityLevelJunctions)
                     .ThenInclude(j => j.Seniority);
        }
    }
}
