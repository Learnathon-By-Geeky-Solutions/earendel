using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Create.v1;

public sealed class CreateInterviewFeedbackHandler(
    ILogger<CreateInterviewFeedbackHandler> logger,
    [FromKeyedServices("interviewfeedbacks:interviewfeedback")] IRepository<InterviewFeedback> repository)
    : IRequestHandler<CreateInterviewFeedbackCommand, CreateInterviewFeedbackResponse>
{
    public async Task<CreateInterviewFeedbackResponse> Handle(CreateInterviewFeedbackCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Create InterviewFeedback object
        var interviewFeedback = InterviewFeedback.Create(
            request.InterviewId,
            // request.InterviewQuestionId,
            request.InterviewQuestionText,
            request.Response,
            request.Score
        );

        await repository.AddAsync(interviewFeedback, cancellationToken);
        logger.LogInformation("Interview feedback created with ID: {InterviewFeedbackId}", interviewFeedback.Id);

        return new CreateInterviewFeedbackResponse(interviewFeedback.Id);
    }
}
