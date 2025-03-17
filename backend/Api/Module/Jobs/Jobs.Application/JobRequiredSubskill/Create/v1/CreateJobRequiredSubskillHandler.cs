using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Create.v1
{
    public sealed class CreateJobRequiredSubskillHandler(
        ILogger<CreateJobRequiredSubskillHandler> logger,
        [FromKeyedServices("jobs:jobrequiredsubskill")] IRepository<Domain.JobRequiredSubskill> repository)
        : IRequestHandler<CreateJobRequiredSubskillCommand, CreateJobRequiredSubskillResponse>
    {
        public async Task<CreateJobRequiredSubskillResponse> Handle(CreateJobRequiredSubskillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var jobRequiredSubskill = Domain.JobRequiredSubskill.Create(request.JobId, request.SubskillId);
            await repository.AddAsync(jobRequiredSubskill, cancellationToken);
            logger.LogInformation("JobRequiredSubskill created with id: {Id}", jobRequiredSubskill.Id);
            return new CreateJobRequiredSubskillResponse(jobRequiredSubskill.Id);
        }
    }
}
