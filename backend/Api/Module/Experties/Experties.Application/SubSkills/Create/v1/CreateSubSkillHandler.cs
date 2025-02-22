using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.SubSkills.Create.v1;
public sealed class CreateSubSkillHandler(
    ILogger<CreateSubSkillHandler> logger,
    [FromKeyedServices("subskills:subskill")] IRepository<Experties.Domain.SubSkill> repository)
    : IRequestHandler<CreateSubSkillCommand, CreateSubSkillResponse>
{
    public async Task<CreateSubSkillResponse> Handle(CreateSubSkillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var skill = Experties.Domain.SubSkill.Create(request.Name!, request.Description, request.SkillId);
        await repository.AddAsync(skill, cancellationToken);
        logger.LogInformation("sub-skill created {SkillId}", skill.Id);
        return new CreateSubSkillResponse(skill.Id);
    }
}
