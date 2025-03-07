using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Interviews.Domain;
using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Get.v1;

public sealed class GetInterviewFeedbackHandler(
    [FromKeyedServices("interviewfeedbacks:interviewfeedbackReadOnly")] IReadRepository<InterviewFeedback> repository,
    ICacheService cache)
    : IRequestHandler<GetInterviewFeedbackRequest, InterviewFeedbackResponse>
{
    public async Task<InterviewFeedbackResponse> Handle(GetInterviewFeedbackRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"interviewfeedback:{request.Id}", // More accurate cache key
            async () =>
            {
                var feedback = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (feedback == null)
                    throw new InterviewFeedbackNotFoundException(request.Id);

                return new InterviewFeedbackResponse(
                    feedback.Id,
                    feedback.InterviewId,
                    feedback.InterviewQuestionId,
                    feedback.Response,
                    feedback.Score
                );
            },
            cancellationToken: cancellationToken
        );

        return item!;
    }
}
