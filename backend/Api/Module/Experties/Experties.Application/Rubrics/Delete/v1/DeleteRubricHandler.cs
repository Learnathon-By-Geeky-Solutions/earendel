using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Rubrics.Delete.v1;
public sealed class DeleteRubricHandler(
    ILogger<DeleteRubricHandler> logger,
    [FromKeyedServices("rubrics:rubric")] IRepository<Experties.Domain.Rubric> repository)
    : IRequestHandler<DeleteRubricCommand>
{
    public async Task Handle(DeleteRubricCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var rubric = await repository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new RubricNotFoundException(request.Id);
        await repository.DeleteAsync(rubric, cancellationToken);
        logger.LogInformation("rubric with id : {RubricId} deleted", rubric.Id);
    }
}
