using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Application.Interviews.Update.v1;

public sealed class UpdateInterviewHandler(
    ILogger<UpdateInterviewHandler> logger,
    [FromKeyedServices("interviews:interview")] IRepository<Interview> repository)
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
        interview.Update(request.ApplicationId, request.InterviewerId, request.InterviewDate, request.Status, request.Notes, request.MeetingId);

        // Save the updated interview back to the repository
        await repository.UpdateAsync(interview, cancellationToken);

        // Log the update action
        logger.LogInformation("Interview with id: {InterviewId} updated.", interview.Id);

        // Return a response containing the updated interview's ID
        return new UpdateInterviewResponse(interview.Id);
    }
}
