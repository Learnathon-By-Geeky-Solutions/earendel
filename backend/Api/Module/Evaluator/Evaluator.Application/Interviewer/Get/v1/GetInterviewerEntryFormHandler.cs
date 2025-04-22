using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using MediatR;
using TalentMesh.Module.Evaluator.Domain.Exceptions;
using TalentMesh.Module.Evaluator.Domain;

namespace Evaluator.Application.Interviewer.Get.v1
{
    public sealed class GetInterviewerEntryFormHandler(
        [FromKeyedServices("interviews:interviewerentryformReadOnly")] IReadRepository<InterviewerEntryForm> repository,
        ICacheService cache
    ) : IRequestHandler<GetInterviewerEntryFormRequest, InterviewerEntryFormResponse>
    {
        public async Task<InterviewerEntryFormResponse> Handle(GetInterviewerEntryFormRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var item = await cache.GetOrSetAsync(
                $"interviewerentryform:{request.Id}",
                async () =>
                {
                    var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
                    if (entity == null)
                        throw new InterviewerEntryFormNotFoundException(request.Id);

                    return new InterviewerEntryFormResponse(
                        entity.Id,
                        entity.UserId,
                        entity.CV,
                        entity.AdditionalInfo,
                        entity.Status
                    );
                },
                cancellationToken: cancellationToken);
            return item!;
        }
    }
}
