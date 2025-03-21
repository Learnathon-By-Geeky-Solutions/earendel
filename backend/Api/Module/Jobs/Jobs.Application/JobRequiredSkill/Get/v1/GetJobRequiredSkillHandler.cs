using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions; // Ensure JobRequiredSkillNotFoundException is defined.
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Get.v1
{
    public sealed class GetJobRequiredSkillHandler(
        [FromKeyedServices("jobs:jobrequiredskillReadOnly")] IReadRepository<Domain.JobRequiredSkill> repository,
        ICacheService cache)
        : IRequestHandler<GetJobRequiredSkillRequest, JobRequiredSkillResponse>
    {
        public async Task<JobRequiredSkillResponse> Handle(GetJobRequiredSkillRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var item = await cache.GetOrSetAsync(
                $"jobRequiredSkill:{request.Id}",
                async () =>
                {
                    var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
                    if (entity == null || entity.DeletedBy != Guid.Empty)
                        throw new JobRequiredSkillNotFoundException(request.Id);
                    return new JobRequiredSkillResponse(
                        entity.Id,
                        entity.JobId,
                        entity.SkillId
                    );
                },
                cancellationToken: cancellationToken);
            return item!;
        }
    }
}
