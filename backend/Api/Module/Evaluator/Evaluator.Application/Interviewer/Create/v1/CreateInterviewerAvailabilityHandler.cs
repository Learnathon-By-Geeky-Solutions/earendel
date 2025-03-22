using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Evaluator.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

            // Use the specification to fetch all existing availabilities for the interviewer.
            var spec = new InterviewerAvailabilityByInterviewerIdSpec(request.InterviewerId);
            var existingAvailabilities = await repository.ListAsync(spec, cancellationToken);

            var newAvailabilities = new List<InterviewerAvailability>();

            foreach (var slot in request.AvailabilitySlots)
            {
                if (IsOverlapping(slot, existingAvailabilities))
                {
                    logger.LogWarning("Interviewer {InterviewerId} has an overlapping availability slot {StartTime} - {EndTime}",
                        request.InterviewerId, slot.StartTime, slot.EndTime);
                    throw new InvalidOperationException($"Interviewer already has an availability in this time range: {slot.StartTime} - {slot.EndTime}");
                }

                var availability = InterviewerAvailability.Create(request.InterviewerId, slot.StartTime, slot.EndTime, true);
                newAvailabilities.Add(availability);
            }

            await repository.AddRangeAsync(newAvailabilities, cancellationToken);

            logger.LogInformation("Created {Count} new availability slots for interviewer {InterviewerId}",
                newAvailabilities.Count, request.InterviewerId);

            return new CreateInterviewerAvailabilityResponse(newAvailabilities.Select(a => a.Id).ToList());
        }

        private static bool IsOverlapping(AvailabilitySlot slot, IEnumerable<InterviewerAvailability> existingAvailabilities)
        {
            return existingAvailabilities.Any(existing =>
                slot.StartTime < existing.EndTime && existing.StartTime < slot.EndTime);
        }
    }
}