using TalentMesh.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Evaluator.Domain.Exceptions;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1
{
    public sealed class UpdateInterviewerEntryFormHandler(
        ILogger<UpdateInterviewerEntryFormHandler> logger,
        [FromKeyedServices("interviews:interviewerentryform")] IRepository<InterviewerEntryForm> repository)
        : IRequestHandler<UpdateInterviewerEntryFormCommand, UpdateInterviewerEntryFormResponse>
    {
        public async Task<UpdateInterviewerEntryFormResponse> Handle(UpdateInterviewerEntryFormCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                throw new InterviewerEntryFormNotFoundException(request.Id);
            }
            var updatedEntity = entity.Update(request.AdditionalInfo, request.Status);
            await repository.UpdateAsync(updatedEntity, cancellationToken);
            logger.LogInformation("User with id {Id}.", request.UserId);
            logger.LogInformation("InterviewerEntryForm with id {Id} updated.", entity.Id);
            return new UpdateInterviewerEntryFormResponse(entity.Id);
        }
    }
}
