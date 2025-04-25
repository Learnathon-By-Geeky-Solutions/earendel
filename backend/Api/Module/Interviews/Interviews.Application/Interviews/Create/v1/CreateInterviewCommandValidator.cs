using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.Interviews.Create.v1;

[ExcludeFromCodeCoverage]

public class CreateInterviewCommandValidator : AbstractValidator<CreateInterviewCommand>
{
    public CreateInterviewCommandValidator()
    {
        RuleFor(b => b.ApplicationId)
            .NotEmpty().WithMessage("ApplicationId is required.");

        RuleFor(b => b.InterviewerId)
            .NotEmpty().WithMessage("InterviewerId is required.");

        RuleFor(b => b.CandidateId)
            .NotEmpty().WithMessage("CandidateId is required.");

        RuleFor(b => b.JobId)
            .NotEmpty().WithMessage("JobId is required.");

        RuleFor(b => b.InterviewDate)
            .NotEmpty().WithMessage("InterviewDate is required.")
            .GreaterThan(DateTime.UtcNow).WithMessage("InterviewDate must be in the future.");

        RuleFor(b => b.Status)
            .NotEmpty().WithMessage("Status is required.")
            .MaximumLength(50).WithMessage("Status must not exceed 50 characters.");

        RuleFor(b => b.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.");

        // New rule for MeetingId
        RuleFor(b => b.MeetingId)
            .NotEmpty().WithMessage("MeetingId is required.")
            .MaximumLength(100).WithMessage("MeetingId must not exceed 100 characters.");
    }
}
