using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Job.Domain.Extentsion;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Delete.v1
{
    public sealed class DeleteJobRequiredSkillHandler(
        ILogger<DeleteJobRequiredSkillHandler> logger,
        [FromKeyedServices("jobs:jobrequiredskill")] IRepository<Domain.JobRequiredSkill> repository)
        : IRequestHandler<DeleteJobRequiredSkillCommand>
    {
        public async Task Handle(DeleteJobRequiredSkillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var entity = await repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new JobRequiredSkillNotFoundException(request.Id);

            await repository.DeleteAsync(entity!, cancellationToken);
            logger.LogInformation("JobRequiredSkill with id {Id} deleted", entity.Id);
        }
    }
}
