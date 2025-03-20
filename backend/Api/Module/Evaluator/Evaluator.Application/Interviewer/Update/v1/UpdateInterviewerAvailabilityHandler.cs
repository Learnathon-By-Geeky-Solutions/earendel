using TalentMesh.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Evaluator.Domain.Exceptions;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1
{
    public sealed class UpdateInterviewerAvailabilityHandler(
        ILogger<UpdateInterviewerAvailabilityHandler> logger,
        [FromKeyedServices("interviews:intervieweravailability")] IRepository<InterviewerAvailability> repository)
        : IRequestHandler<UpdateInterviewerAvailabilityCommand, UpdateInterviewerAvailabilityResponse>
    {
        public async Task<UpdateInterviewerAvailabilityResponse> Handle(UpdateInterviewerAvailabilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                throw new InterviewerAvailabilityNotFoundException(request.Id);
            }
            var updatedEntity = entity.Update(request.StartTime, request.EndTime, request.IsAvailable);
            await repository.UpdateAsync(updatedEntity, cancellationToken);
            logger.LogInformation("InterviewerAvailability with id {Id} updated.", entity.Id);
            return new UpdateInterviewerAvailabilityResponse(entity.Id);
        }
    }
}
