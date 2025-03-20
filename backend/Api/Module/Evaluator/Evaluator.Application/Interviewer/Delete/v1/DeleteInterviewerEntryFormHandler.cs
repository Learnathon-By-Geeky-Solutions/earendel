using TalentMesh.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Evaluator.Application.Interviewer.Delete.v1;
using TalentMesh.Module.Evaluator.Domain.Exceptions;
using TalentMesh.Module.Evaluator.Domain;
using TalentMesh.Module.Evaluator.Domain.Extensions;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Delete.v1
{
    public sealed class DeleteInterviewerEntryFormHandler(
        ILogger<DeleteInterviewerEntryFormHandler> logger,
        [FromKeyedServices("interviews:interviewerentryform")] IRepository<InterviewerEntryForm> repository)
        : IRequestHandler<DeleteInterviewerEntryFormCommand>
    {
        public async Task Handle(DeleteInterviewerEntryFormCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var entryForm = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (entryForm == null)
                throw new InterviewerEntryFormNotFoundException(request.Id);

            await repository.DeleteAsync(entryForm, cancellationToken);
            logger.LogInformation("InterviewerEntryForm with id {EntryFormId} deleted", entryForm.Id);
        }
    }
}
