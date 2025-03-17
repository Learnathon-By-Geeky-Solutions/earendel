using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Evaluator.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    public sealed class CreateInterviewerEntryFormHandler(
        ILogger<CreateInterviewerEntryFormHandler> logger,
        [FromKeyedServices("interviews:interviewerentryform")] IRepository<InterviewerEntryForm> repository)
        : IRequestHandler<CreateInterviewerEntryFormCommand, CreateInterviewerEntryFormResponse>
    {
        public async Task<CreateInterviewerEntryFormResponse> Handle(CreateInterviewerEntryFormCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var entryForm = InterviewerEntryForm.Create(request.UserId, request.AdditionalInfo);
            await repository.AddAsync(entryForm, cancellationToken);
            logger.LogInformation("InterviewerEntryForm created {EntryFormId}", entryForm.Id);
            return new CreateInterviewerEntryFormResponse(entryForm.Id);
        }
    }
}
