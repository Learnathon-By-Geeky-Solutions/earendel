using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Rubrics.Create.v1;
public sealed class CreateRubricHandler(
    ILogger<CreateRubricHandler> logger,
    [FromKeyedServices("rubrics:rubric")] IRepository<Experties.Domain.Rubric> repository)
    : IRequestHandler<CreateRubricCommand, CreateRubricResponse>
{
    public async Task<CreateRubricResponse> Handle(CreateRubricCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var rubric = Experties.Domain.Rubric.Create(request.Title, request.RubricDescription, request.SubSkillId, request.SeniorityLevelId, request.Weight);
        await repository.AddAsync(rubric, cancellationToken);
        logger.LogInformation("rubric created {RubricId}", rubric.Id);
        return new CreateRubricResponse(rubric.Id);
    }
}
