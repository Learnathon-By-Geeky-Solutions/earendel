using System;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Experties.Application.Seniorities.Update.v1
{
    public sealed class UpdateSeniorityHandler(
        ILogger<UpdateSeniorityHandler> logger,
        [FromKeyedServices("seniorities:seniority")] IRepository<Seniority> repository)
        : IRequestHandler<UpdateSeniorityCommand, UpdateSeniorityResponse>
    {
        public async Task<UpdateSeniorityResponse> Handle(UpdateSeniorityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // Retrieve or throw immediately (no branching)
            var seniority = await repository.GetByIdAsync(request.Id, cancellationToken)
                            ?? throw new SeniorityNotFoundException(request.Id);

            // Update and persist
            var updated = seniority.Update(request.Name, request.Description);
            await repository.UpdateAsync(updated, cancellationToken);

            logger.LogInformation("Seniority with ID: {SeniorityId} updated.", updated.Id);
            return new UpdateSeniorityResponse(updated.Id);
        }
    }
}
