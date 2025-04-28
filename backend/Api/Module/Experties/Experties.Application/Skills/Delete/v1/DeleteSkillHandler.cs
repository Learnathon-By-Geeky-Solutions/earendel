using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Skills.Delete.v1;

public sealed class DeleteSkillHandler(
    ILogger<DeleteSkillHandler> logger,
    [FromKeyedServices("skills:skill")] IRepository<Skill> repository)
    : IRequestHandler<DeleteSkillCommand>
{
    public async Task Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var skill = await repository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new SkillNotFoundException(request.Id); // Inline null check removes conditional branching

        await repository.DeleteAsync(skill, cancellationToken);
        logger.LogInformation("Skill with id : {SkillId} deleted", skill.Id);
    }
}
