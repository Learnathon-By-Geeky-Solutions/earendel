using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Rubrics.Update.v1;
public sealed class UpdateRubricHandler(
    ILogger<UpdateRubricHandler> logger,
    [FromKeyedServices("rubrics:rubric")] IRepository<Experties.Domain.Rubric> repository)
    : IRequestHandler<UpdateRubricCommand, UpdateRubricResponse>
{
    public async Task<UpdateRubricResponse> Handle(UpdateRubricCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var skill = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (skill is null || skill.DeletedBy is not null)
        {
            throw new RubricNotFoundException(request.Id);
        }
        var updatedSkill = skill.Update(request.Title, request.RubricDescription, request.SubSkillId, request.SeniorityLevelId, request.Weight);
        await repository.UpdateAsync(updatedSkill, cancellationToken);
        logger.LogInformation("Rubric with id : {Rubric} updated.", skill.Id);
        return new UpdateRubricResponse(skill.Id);
    }
}
