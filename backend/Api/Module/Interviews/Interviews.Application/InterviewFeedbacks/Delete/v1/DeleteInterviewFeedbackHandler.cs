using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Delete.v1;

public sealed class DeleteInterviewFeedbackHandler(
    ILogger<DeleteInterviewFeedbackHandler> logger,
    [FromKeyedServices("interviewfeedbacks:interviewfeedback")] IRepository<InterviewFeedback> repository)
    : IRequestHandler<DeleteInterviewFeedbackCommand>
{
    public async Task Handle(DeleteInterviewFeedbackCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var interviewFeedback = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (interviewFeedback == null)
            throw new InterviewFeedbackNotFoundException(request.Id);

        await repository.DeleteAsync(interviewFeedback, cancellationToken);
        logger.LogInformation("Interview feedback with ID: {InterviewFeedbackId} deleted", interviewFeedback.Id);
    }
}
