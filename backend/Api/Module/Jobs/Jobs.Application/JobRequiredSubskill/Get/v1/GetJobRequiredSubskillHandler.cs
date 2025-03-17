using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions; // Ensure JobRequiredSubskillNotFoundException is defined.
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Get.v1
{
    public sealed class GetJobRequiredSubskillHandler(
        [FromKeyedServices("jobs:jobrequiredsubskillReadOnly")] IReadRepository<Domain.JobRequiredSubskill> repository,
        ICacheService cache)
        : IRequestHandler<GetJobRequiredSubskillRequest, JobRequiredSubskillResponse>
    {
        public async Task<JobRequiredSubskillResponse> Handle(GetJobRequiredSubskillRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var item = await cache.GetOrSetAsync(
                $"jobRequiredSubskill:{request.Id}",
                async () =>
                {
                    var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
                    if (entity == null || entity.DeletedBy != Guid.Empty)
                        throw new JobRequiredSubskillNotFoundException(request.Id);
                    return new JobRequiredSubskillResponse(
                        entity.Id,
                        entity.JobId,
                        entity.SubskillId
                    );
                },
                cancellationToken: cancellationToken);
            return item!;
        }
    }
}
