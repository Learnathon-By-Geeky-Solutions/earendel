using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Framework.Infrastructure.Messaging;

namespace TalentMesh.Module.Interviews.Application.Interviews.Update.v1;

public sealed class UpdateInterviewHandler(
    ILogger<UpdateInterviewHandler> logger,
    [FromKeyedServices("interviews:interview")] IRepository<Interview> repository, IMessageBus messageBus)
    : IRequestHandler<UpdateInterviewCommand, UpdateInterviewResponse>
{
    public async Task<UpdateInterviewResponse> Handle(UpdateInterviewCommand request, CancellationToken cancellationToken)
    {
        // Ensure the request is not null
        ArgumentNullException.ThrowIfNull(request);

        // Fetch the existing interview from the repository
        var interview = await repository.GetByIdAsync(request.Id, cancellationToken);

        // Check if the interview exists
        if (interview is null)
        {
            throw new InterviewNotFoundException(request.Id);
        }

        // Update the interview entity with the new fields
        interview.Update(request.ApplicationId, request.InterviewerId, request.CandidateId, request.JobId, request.InterviewDate, request.Status, request.Notes, request.MeetingId);

        // Save the updated interview back to the repository
        await repository.UpdateAsync(interview, cancellationToken);

        var notificationMessage = new 
        {
            UserId = "null",
            Entity = request.JobId, 
            EntityType = "InterviewScheduled.HR",
            Message = $"Interviewer {request.InterviewerId} has accepted the interview for Job {request.JobId} with Candidate {request.CandidateId} scheduled at {request.InterviewDate:yyyy-MM-dd HH:mm}."
        };

        await messageBus.PublishAsync(notificationMessage, "notification.events", "notification.fetched", cancellationToken);


        var candidateNotification = new
        {
            UserId = request.CandidateId,
            Entity = request.JobId,
            EntityType = "InterviewScheduled.Candidate",    // ← distinct type
            Message = $"Your interview for Job {request.JobId} has been accepted by Interviewer {request.InterviewerId}, scheduled at {request.InterviewDate:yyyy-MM-dd HH:mm}."
        };

        await messageBus.PublishAsync(candidateNotification, "notification.events", "notification.fetched", cancellationToken);


        // Log the update action
        logger.LogInformation("Interview with id: {InterviewId} updated.", interview.Id);

        // Return a response containing the updated interview's ID
        return new UpdateInterviewResponse(interview.Id);
    }
}
