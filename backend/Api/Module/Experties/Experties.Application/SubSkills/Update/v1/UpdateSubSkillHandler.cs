using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.SubSkills.Update.v1;
public sealed class UpdateSubSkillHandler(
    ILogger<UpdateSubSkillHandler> logger,
    [FromKeyedServices("subskills:subskill")] IRepository<Experties.Domain.SubSkill> repository)
    : IRequestHandler<UpdateSubSkillCommand, UpdateSubSkillResponse>
{
    public async Task<UpdateSubSkillResponse> Handle(UpdateSubSkillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var skill = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (skill is null)
        {
            throw new SubSkillNotFoundException(request.Id);
        }
        var updatedSkill = skill.Update(request.Name, request.Description, request.SkillId);
        await repository.UpdateAsync(updatedSkill, cancellationToken);
        logger.LogInformation("subskill with id : {SubSkill} updated.", skill.Id);
        return new UpdateSubSkillResponse(skill.Id);
    }
}
