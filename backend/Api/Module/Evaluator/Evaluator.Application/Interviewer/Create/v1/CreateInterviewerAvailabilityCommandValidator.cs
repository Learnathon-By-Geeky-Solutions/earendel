using FluentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    [ExcludeFromCodeCoverage]
    public class CreateInterviewerAvailabilityCommandValidator : AbstractValidator<CreateInterviewerAvailabilityCommand>
    {
        public CreateInterviewerAvailabilityCommandValidator()
        {
            RuleFor(x => x.InterviewerId)
                .NotEmpty().WithMessage("InterviewerId must be provided.");

            RuleFor(x => x.AvailabilitySlots)
                .NotEmpty().WithMessage("AvailabilitySlots must be provided.")
                .Must(slots => slots.Count > 0)
                .WithMessage("At least one availability slot must be provided.")
                .Must(NoOverlappingSlots)
                .WithMessage("Availability slots must not overlap.");

            RuleForEach(x => x.AvailabilitySlots).ChildRules(slot =>
            {
                slot.RuleFor(x => x.StartTime)
                    .NotEmpty().WithMessage("StartTime must be provided.")
                    .Must(BeWithinLastSixHoursBangladeshTime)
                    .WithMessage("StartTime cannot be more than 6 hours in the past (BST).");

                slot.RuleFor(x => x.EndTime)
                    .NotEmpty().WithMessage("EndTime must be provided.")
                    .GreaterThan(x => x.StartTime).WithMessage("EndTime must be after StartTime.")
                    .Must((slot, endTime) => (endTime - slot.StartTime).TotalMinutes >= 60)
                    .WithMessage("Each slot must be at least 1 hour.");
            });
        }

        private static bool NoOverlappingSlots(List<AvailabilitySlot> slots)
        {
            return !slots.Any(slot1 => slots.Any(slot2 =>
                slot1 != slot2 &&
                slot1.StartTime < slot2.EndTime &&
                slot2.StartTime < slot1.EndTime
            ));
        }

        /// <summary>
        /// Validates that StartTime is no earlier than 6 hours before now in Bangladesh Standard Time.
        /// </summary>
        private static bool BeWithinLastSixHoursBangladeshTime(DateTime startTime)
        {
            // Compute current BST (UTC+6)
            var nowUtc = DateTime.UtcNow;
            var nowBst = nowUtc.AddHours(6);

            // Earliest allowable time is 6 hours before BST now
            var earliestAllowed = nowBst.AddHours(-6);

            return startTime >= earliestAllowed;
        }
    }
}
