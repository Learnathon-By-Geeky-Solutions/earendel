using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Application.Interviews.Delete.v1;

public sealed class DeleteInterviewHandler(
    ILogger<DeleteInterviewHandler> logger,
    [FromKeyedServices("interviews:interview")] IRepository<Interview> repository)
    : IRequestHandler<DeleteInterviewCommand>
{
    public async Task Handle(DeleteInterviewCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var interview = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (interview == null)
            throw new InterviewNotFoundException(request.Id);

        await repository.DeleteAsync(interview, cancellationToken);
        logger.LogInformation("Interview with ID: {InterviewId} deleted", interview.Id);
    }
}
