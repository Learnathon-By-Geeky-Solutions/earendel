using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Seniorities.Create.v1;
public sealed class CreateSeniorityHandler(
    ILogger<CreateSeniorityHandler> logger,
    [FromKeyedServices("seniorities:seniority")] IRepository<Experties.Domain.Seniority> repository)
    : IRequestHandler<CreateSeniorityCommand, CreateSeniorityResponse>
{
    public async Task<CreateSeniorityResponse> Handle(CreateSeniorityCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var seniority = Experties.Domain.Seniority.Create(request.Name!, request.Description);
        await repository.AddAsync(seniority, cancellationToken);
        logger.LogInformation("seniority created {SeniorityId}", seniority.Id);
        return new CreateSeniorityResponse(seniority.Id);
    }
}
