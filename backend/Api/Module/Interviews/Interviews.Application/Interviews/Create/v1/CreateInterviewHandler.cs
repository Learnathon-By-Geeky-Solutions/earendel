using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Interviews.Application.Services;

namespace TalentMesh.Module.Interviews.Application.Interviews.Create.v1;

public sealed class CreateInterviewHandler(
    ILogger<CreateInterviewHandler> logger,
    [FromKeyedServices("interviews:interview")] IRepository<Interview> repository, IZoomService zoomService)
    : IRequestHandler<CreateInterviewCommand, CreateInterviewResponse>
{
    public async Task<CreateInterviewResponse> Handle(CreateInterviewCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var accessToken = await zoomService.GetAccessTokenAsync();

        string meetingId = await zoomService.CreateZoomMeetingAsync(accessToken, request.InterviewDate);
        logger.LogInformation("Created Zoom meeting with ID: {MeetingId}", meetingId);

        // Pass MeetingId to the Create method
        var interview = Interview.Create(
            request.ApplicationId,
            request.InterviewerId,
            request.CandidateId,
            request.JobId,
            request.InterviewDate,
            request.Status,
            request.Notes,
            meetingId // Include MeetingId
        );

        await repository.AddAsync(interview, cancellationToken);
        logger.LogInformation("Interview created with ID: {InterviewId}", interview.Id);

        return new CreateInterviewResponse(interview.Id);
    }
}
