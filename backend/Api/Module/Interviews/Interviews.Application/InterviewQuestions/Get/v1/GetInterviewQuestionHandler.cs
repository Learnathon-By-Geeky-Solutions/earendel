using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Interviews.Domain.Exceptions; // Ensure this exception exists
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Interviews.Domain;
using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Get.v1;

public sealed class GetInterviewQuestionHandler(
    [FromKeyedServices("interviewquestions:interviewquestionReadOnly")] IReadRepository<InterviewQuestion> repository,
    ICacheService cache)
    : IRequestHandler<GetInterviewQuestionRequest, InterviewQuestionResponse>
{
    public async Task<InterviewQuestionResponse> Handle(GetInterviewQuestionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"interviewquestion:{request.Id}", // Updated key for InterviewQuestion
            async () =>
            {
                var interviewQuestion = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (interviewQuestion == null)
                    throw new InterviewQuestionNotFoundException(request.Id);

                return new InterviewQuestionResponse(
                    interviewQuestion.Id,
                    interviewQuestion.RubricId,
                    interviewQuestion.QuestionText
                );
            },
            cancellationToken: cancellationToken
        );

        return item!;
    }
}
