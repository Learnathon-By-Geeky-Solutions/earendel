using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Create.v1
{
    public sealed class CreateJobRequiredSkillHandler(
        ILogger<CreateJobRequiredSkillHandler> logger,
        [FromKeyedServices("jobs:jobrequiredskill")] IRepository<Domain.JobRequiredSkill> repository)
        : IRequestHandler<CreateJobRequiredSkillCommand, CreateJobRequiredSkillResponse>
    {
        public async Task<CreateJobRequiredSkillResponse> Handle(CreateJobRequiredSkillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var jobRequiredSkill = Domain.JobRequiredSkill.Create(request.JobId, request.SkillId);
            await repository.AddAsync(jobRequiredSkill, cancellationToken);
            logger.LogInformation("JobRequiredSkill created with id: {Id}", jobRequiredSkill.Id);
            return new CreateJobRequiredSkillResponse(jobRequiredSkill.Id);
        }
    }
}
