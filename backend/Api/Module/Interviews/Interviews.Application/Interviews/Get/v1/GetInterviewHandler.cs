using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Interviews.Domain;
using MediatR;

namespace TalentMesh.Module.Interviews.Application.Interviews.Get.v1;

public sealed class GetInterviewHandler(
    [FromKeyedServices("interviews:interviewReadOnly")] IReadRepository<Interview> repository,
    ICacheService cache)
    : IRequestHandler<GetInterviewRequest, InterviewResponse>
{
    public async Task<InterviewResponse> Handle(GetInterviewRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"interview:{request.Id}", // Updated key from "notification" to "interview"
            async () =>
            {
                var interviewItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (interviewItem == null)
                    throw new InterviewNotFoundException(request.Id);

                return new InterviewResponse(
                    interviewItem.Id,
                    interviewItem.ApplicationId,
                    interviewItem.InterviewerId,
                    interviewItem.InterviewDate,
                    interviewItem.Status,
                    interviewItem.Notes,
                    interviewItem.MeetingId // Include MeetingId
                );
            },
            cancellationToken: cancellationToken
        );

        return item!;
    }
}
