using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Experties.Domain;
using MediatR;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Experties.Application.Skills.Get.v1
{
    public sealed class GetSkillHandler(
        [FromKeyedServices("skills:skillReadOnly")] IReadRepository<Skill> repository,
        ICacheService cache
    ) : IRequestHandler<GetSkillRequest, SkillResponse>
    {
        public async Task<SkillResponse> Handle(GetSkillRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // Use the specification to include related data
            var spec = new GetSkillSpec(request.Id);
            var skillItem = await repository.GetBySpecAsync(spec, cancellationToken);
            if (skillItem == null)
            {
                throw new SkillNotFoundException(request.Id);
            }

            // Map related subskills
            var subSkills = skillItem.SubSkills.Select(sub => new SubSkillResponse(
                sub.Id,
                sub.Name,
                sub.Description,
                sub.SkillId
            )).ToList();

            // Map related seniority junctions (with associated seniority details)
            var seniorityLevelJunctions = skillItem.SeniorityLevelJunctions.Select(j => new SeniorityLevelJunctionResponse(
                j.Id,
                j.Seniority.Id,
                j.SkillId,
                new TalentMesh.Module.Experties.Application.Seniorities.Get.v1.SeniorityResponse(
                    j.Seniority.Id,
                    j.Seniority.Name,
                    j.Seniority.Description
                )
            )).ToList();

            return new SkillResponse(skillItem.Id, skillItem.Name, skillItem.Description, subSkills, seniorityLevelJunctions);
        }
    }
}
