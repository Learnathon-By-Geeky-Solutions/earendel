using TalentMesh.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Evaluator.Domain;
using TalentMesh.Module.Evaluator.Domain.Exceptions;
using TalentMesh.Module.Evaluator.Domain.Extensions;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Delete.v1
{
    public sealed class DeleteInterviewerApplicationHandler(
        ILogger<DeleteInterviewerApplicationHandler> logger,
        [FromKeyedServices("interviews:interviewerapplication")] IRepository<InterviewerApplication> repository)
        : IRequestHandler<DeleteInterviewerApplicationCommand>
    {
        public async Task Handle(DeleteInterviewerApplicationCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var application = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (application == null)
                throw new InterviewerApplicationNotFoundException(request.Id);

            await repository.DeleteAsync(application, cancellationToken);
            logger.LogInformation("InterviewerApplication with id {ApplicationId} deleted", application.Id);
        }
    }
}
