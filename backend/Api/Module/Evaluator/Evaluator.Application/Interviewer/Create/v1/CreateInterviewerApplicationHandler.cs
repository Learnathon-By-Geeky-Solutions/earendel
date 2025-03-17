using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Evaluator.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    public sealed class CreateInterviewerApplicationHandler(
        ILogger<CreateInterviewerApplicationHandler> logger,
        [FromKeyedServices("interviews:interviewerapplication")] IRepository<InterviewerApplication> repository)
        : IRequestHandler<CreateInterviewerApplicationCommand, CreateInterviewerApplicationResponse>
    {
        public async Task<CreateInterviewerApplicationResponse> Handle(CreateInterviewerApplicationCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var application = InterviewerApplication.Create(request.JobId, request.InterviewerId, request.Comments);
            await repository.AddAsync(application, cancellationToken);
            logger.LogInformation("InterviewerApplication created {ApplicationId}", application.Id);
            return new CreateInterviewerApplicationResponse(application.Id);
        }
    }
}
