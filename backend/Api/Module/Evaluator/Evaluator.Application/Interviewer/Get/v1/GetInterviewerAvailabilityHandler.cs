using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using MediatR;
using TalentMesh.Module.Evaluator.Domain.Exceptions;
using TalentMesh.Module.Evaluator.Domain;

namespace Evaluator.Application.Interviewer.Get.v1
{
    public sealed class GetInterviewerAvailabilityHandler(
        [FromKeyedServices("interviews:intervieweravailabilityReadOnly")] IReadRepository<InterviewerAvailability> repository,
        ICacheService cache
    ) : IRequestHandler<GetInterviewerAvailabilityRequest, InterviewerAvailabilityResponse>
    {
        public async Task<InterviewerAvailabilityResponse> Handle(GetInterviewerAvailabilityRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var item = await cache.GetOrSetAsync(
                $"intervieweravailability:{request.Id}",
                async () =>
                {
                    var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
                    if (entity == null || entity.DeletedBy != Guid.Empty)
                        throw new InterviewerAvailabilityNotFoundException(request.Id);

                    return new InterviewerAvailabilityResponse(
                        entity.Id,
                        entity.InterviewerId,
                        entity.StartTime,
                        entity.EndTime,
                        entity.IsAvailable
                    );
                },
                cancellationToken: cancellationToken);
            return item!;
        }
    }
}
