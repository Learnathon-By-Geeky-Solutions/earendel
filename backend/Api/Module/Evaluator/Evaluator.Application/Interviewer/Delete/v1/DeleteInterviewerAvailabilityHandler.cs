using TalentMesh.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Evaluator.Domain.Exceptions;
using TalentMesh.Module.Evaluator.Domain;
using TalentMesh.Module.Evaluator.Domain.Extensions;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Delete.v1
{
    public sealed class DeleteInterviewerAvailabilityHandler(
        ILogger<DeleteInterviewerAvailabilityHandler> logger,
        [FromKeyedServices("interviews:intervieweravailability")] IRepository<InterviewerAvailability> repository)
        : IRequestHandler<DeleteInterviewerAvailabilityCommand>
    {
        public async Task Handle(DeleteInterviewerAvailabilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var availability = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (availability == null)
                throw new InterviewerAvailabilityNotFoundException(request.Id);

            await repository.DeleteAsync(availability, cancellationToken);
            logger.LogInformation("InterviewerAvailability with id {AvailabilityId} deleted", availability.Id);
        }
    }
}
