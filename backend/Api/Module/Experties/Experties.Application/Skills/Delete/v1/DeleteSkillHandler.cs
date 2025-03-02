using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Skills.Delete.v1;
public sealed class DeleteSkillHandler(
    ILogger<DeleteSkillHandler> logger,
    [FromKeyedServices("skills:skill")] IRepository<Experties.Domain.Skill> repository)
    : IRequestHandler<DeleteSkillCommand>
{
    public async Task Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var skill = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (skill == null || skill.DeletedBy is not null) throw new SkillNotFoundException(request.Id);
        await repository.DeleteAsync(skill, cancellationToken);
        logger.LogInformation("Skill with id : {SkillId} deleted", skill.Id);
    }
}
