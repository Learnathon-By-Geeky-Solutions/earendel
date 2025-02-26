using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Skills.Update.v1;
public sealed class UpdateSkillHandler(
    ILogger<UpdateSkillHandler> logger,
    [FromKeyedServices("skills:skill")] IRepository<Experties.Domain.Skill> repository)
    : IRequestHandler<UpdateSkillCommand, UpdateSkillResponse>
{
    public async Task<UpdateSkillResponse> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var skill = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (skill is null || skill.DeletedBy != Guid.Empty)
        {
            throw new SkillNotFoundException(request.Id);
        }
        var updatedSkill = skill.Update(request.Name, request.Description);
        await repository.UpdateAsync(updatedSkill, cancellationToken);
        logger.LogInformation("skill with id : {SkillId} updated.", skill.Id);
        return new UpdateSkillResponse(skill.Id);
    }
}
