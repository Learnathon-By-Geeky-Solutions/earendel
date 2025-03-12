using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Evaluator.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    public sealed class CreateInterviewerAvailabilityHandler(
        ILogger<CreateInterviewerAvailabilityHandler> logger,
        [FromKeyedServices("interviews:intervieweravailability")] IRepository<InterviewerAvailability> repository)
        : IRequestHandler<CreateInterviewerAvailabilityCommand, CreateInterviewerAvailabilityResponse>
    {
        public async Task<CreateInterviewerAvailabilityResponse> Handle(CreateInterviewerAvailabilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var availability = InterviewerAvailability.Create(request.InterviewerId, request.StartTime, request.EndTime, request.IsAvailable);
            await repository.AddAsync(availability, cancellationToken);
            logger.LogInformation("InterviewerAvailability created {AvailabilityId}", availability.Id);
            return new CreateInterviewerAvailabilityResponse(availability.Id);
        }
    }
}
