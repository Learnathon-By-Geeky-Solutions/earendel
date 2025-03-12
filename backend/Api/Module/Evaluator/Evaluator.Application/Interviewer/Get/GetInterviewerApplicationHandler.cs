using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using MediatR;
using TalentMesh.Module.Evaluator.Domain.Exceptions;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Get.v1
{
    public sealed class GetInterviewerApplicationHandler(
        [FromKeyedServices("interviews:interviewerapplicationReadOnly")] IReadRepository<InterviewerApplication> repository,
        ICacheService cache
    ) : IRequestHandler<GetInterviewerApplicationRequest, InterviewerApplicationResponse>
    {
        public async Task<InterviewerApplicationResponse> Handle(GetInterviewerApplicationRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var item = await cache.GetOrSetAsync(
                $"interviewerapplication:{request.Id}",
                async () =>
                {
                    var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
                    if (entity == null || entity.DeletedBy != Guid.Empty)
                        throw new InterviewerApplicationNotFoundException(request.Id);

                    return new InterviewerApplicationResponse(
                        entity.Id,
                        entity.JobId,
                        entity.InterviewerId,
                        entity.AppliedDate,
                        entity.Status,
                        entity.Comments
                    );
                },
                cancellationToken: cancellationToken);
            return item!;
        }
    }
}
