using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Update.v1;

public sealed class UpdateInterviewFeedbackHandler(
    ILogger<UpdateInterviewFeedbackHandler> logger,
    [FromKeyedServices("interviewfeedbacks:interviewfeedback")] IRepository<InterviewFeedback> repository)
    : IRequestHandler<UpdateInterviewFeedbackCommand, UpdateInterviewFeedbackResponse>
{
    public async Task<UpdateInterviewFeedbackResponse> Handle(UpdateInterviewFeedbackCommand request, CancellationToken cancellationToken)
    {
        // Ensure the request is not null
        ArgumentNullException.ThrowIfNull(request);

        // Fetch the existing interview from the repository
        var interview = await repository.GetByIdAsync(request.Id, cancellationToken);

        // Check if the interview exists
        if (interview is null)
        {
            throw new InterviewFeedbackNotFoundException(request.Id);
        }

        // Update the interview entity with the new fields
        interview.Update(request.InterviewId, request.InterviewQuestionId, request.Response, request.Score);

        // Save the updated interview back to the repository
        await repository.UpdateAsync(interview, cancellationToken);

        // Log the update action
        logger.LogInformation("Interview feedback with id: {InterviewFeedbackId} updated.", interview.Id);

        // Return a response containing the updated interview's ID
        return new UpdateInterviewFeedbackResponse(interview.Id);
    }
}
