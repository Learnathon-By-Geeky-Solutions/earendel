using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.SubSkills.Delete.v1;
public sealed class DeleteSubSkillHandler(
    ILogger<DeleteSubSkillHandler> logger,
    [FromKeyedServices("subskills:subskill")] IRepository<Experties.Domain.SubSkill> repository)
    : IRequestHandler<DeleteSubSkillCommand>
{
    public async Task Handle(DeleteSubSkillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var skill = await repository.GetByIdAsync(request.Id, cancellationToken)
           ?? throw new SubSkillNotFoundException(request.Id);

        await repository.DeleteAsync(skill, cancellationToken);
        logger.LogInformation("sub-skill with id : {SkillId} deleted", skill.Id);
    }
}
