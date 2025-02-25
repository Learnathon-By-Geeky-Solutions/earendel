using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Skills.Create.v1;
public sealed class CreateSkillHandler(
    ILogger<CreateSkillHandler> logger,
    [FromKeyedServices("skills:skill")] IRepository<Experties.Domain.Skill> repository)
    : IRequestHandler<CreateSkillCommand, CreateSkillResponse>
{
    public async Task<CreateSkillResponse> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var skill = Experties.Domain.Skill.Create(request.Name!, request.Description);
        await repository.AddAsync(skill, cancellationToken);
        logger.LogInformation("skill created {SkillId}", skill.Id);
        return new CreateSkillResponse(skill.Id);
    }
}
