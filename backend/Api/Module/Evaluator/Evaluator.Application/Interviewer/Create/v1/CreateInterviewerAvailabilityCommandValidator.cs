using FluentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    [ExcludeFromCodeCoverage]
    public class CreateInterviewerAvailabilityCommandValidator
        : AbstractValidator<CreateInterviewerAvailabilityCommand>
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
                    .Must(BeAtLeastCurrentBangladeshTime)
                        .WithMessage("StartTime cannot be in the past (BST).");

                slot.RuleFor(x => x.EndTime)
                    .NotEmpty().WithMessage("EndTime must be provided.")
                    .GreaterThan(x => x.StartTime)
                        .WithMessage("EndTime must be after StartTime.")
                    .Must((slot, endTime) =>
                        // Ensure at least 1 hour duration in BST terms
                        (NormalizeToBangladesh(endTime) - NormalizeToBangladesh(slot.StartTime)).TotalMinutes >= 60)
                        .WithMessage("Each slot must be at least 1 hour.");
            });
        }

        private static bool NoOverlappingSlots(List<AvailabilitySlot> slots)
        {
            // Compare normalized BST times for overlaps
            return !slots.Any(a => slots.Any(b =>
                a != b &&
                NormalizeToBangladesh(a.StartTime) < NormalizeToBangladesh(b.EndTime) &&
                NormalizeToBangladesh(b.StartTime) < NormalizeToBangladesh(a.EndTime)
            ));
        }

        private static bool BeAtLeastCurrentBangladeshTime(DateTime startTime)
        {
            var startInBST = NormalizeToBangladesh(startTime);
            var nowInBST = DateTime.UtcNow.AddHours(6);
            return startInBST >= nowInBST;
        }

        /// <summary>
        /// Treats the incoming timestamp as UTC, then shifts by +6h to get BST.
        /// </summary>
        private static DateTime NormalizeToBangladesh(DateTime utcOrUnspecified)
        {
            // If the kind is Unspecified or Local, first treat as UTC:
            var asUtc = utcOrUnspecified.Kind switch
            {
                DateTimeKind.Utc => utcOrUnspecified,
                _ => DateTime.SpecifyKind(utcOrUnspecified, DateTimeKind.Utc)
            };
            // Then add 6 hours for BST
            return asUtc.AddHours(6);
        }
    }
}
