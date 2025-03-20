using TalentMesh.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Evaluator.Domain.Exceptions;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1
{
    public sealed class UpdateInterviewerApplicationHandler(
        ILogger<UpdateInterviewerApplicationHandler> logger,
        [FromKeyedServices("interviews:interviewerapplication")] IRepository<InterviewerApplication> repository)
        : IRequestHandler<UpdateInterviewerApplicationCommand, UpdateInterviewerApplicationResponse>
    {
        public async Task<UpdateInterviewerApplicationResponse> Handle(UpdateInterviewerApplicationCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                throw new InterviewerApplicationNotFoundException(request.Id);
            }
            // Assume InterviewerApplication.Update updates JobId, InterviewerId, Status and Comments.
            var updatedEntity = entity.Update(request.Status, request.Comments);
            await repository.UpdateAsync(updatedEntity, cancellationToken);
            logger.LogInformation("InterviewerApplication with id {Id} updated.", entity.Id);
            return new UpdateInterviewerApplicationResponse(entity.Id);
        }
    }
}
