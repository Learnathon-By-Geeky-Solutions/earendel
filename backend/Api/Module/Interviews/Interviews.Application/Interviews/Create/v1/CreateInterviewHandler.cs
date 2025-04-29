using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Interviews.Application.Services;
using TalentMesh.Framework.Infrastructure.Messaging;

namespace TalentMesh.Module.Interviews.Application.Interviews.Create.v1;

public sealed class CreateInterviewHandler(
    ILogger<CreateInterviewHandler> logger,
    [FromKeyedServices("interviews:interview")] IRepository<Interview> repository, IZoomService zoomService, IMessageBus messageBus
)
    : IRequestHandler<CreateInterviewCommand, CreateInterviewResponse>
{
    public async Task<CreateInterviewResponse> Handle(CreateInterviewCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var accessToken = await zoomService.GetAccessTokenAsync();

        string meetingId = await zoomService.CreateZoomMeetingAsync(accessToken, request.InterviewDate);
        logger.LogInformation("Created Zoom meeting with ID: {MeetingId}", meetingId);

        // Pass MeetingId to the Create method
        var details = new InterviewDetails
        {
            ApplicationId = request.ApplicationId,
            InterviewerId = request.InterviewerId,
            CandidateId = request.CandidateId,
            JobId = request.JobId,
            InterviewDate = request.InterviewDate,
            Status = request.Status,
            Notes = request.Notes,
            MeetingId = meetingId
        };

        var interview = Interview.Create(details);

        await repository.AddAsync(interview, cancellationToken);

        var notificationMessage = new
        {
            UserId = request.InterviewerId,
            Entity = request.JobId,
            EntityType = "Interview.Interviewer", 
            Message = $"A new interview request from HR with JobId {request.JobId}, CandidateId {request.CandidateId}.", 
            RequestedBy = request.JobId
        };

        await messageBus.PublishAsync(notificationMessage, "notification.events", "notification.fetched", cancellationToken);

        logger.LogInformation("Interview created with ID: {InterviewId}", interview.Id);

        return new CreateInterviewResponse(interview.Id);
    }
}
