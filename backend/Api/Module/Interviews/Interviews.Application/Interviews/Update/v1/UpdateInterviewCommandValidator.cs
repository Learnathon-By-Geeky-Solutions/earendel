using FluentValidation;

namespace TalentMesh.Module.Interviews.Application.Interviews.Update.v1;

public class UpdateInterviewCommandValidator : AbstractValidator<UpdateInterviewCommand>
{
    public UpdateInterviewCommandValidator()
    {
        // Validate Id (should not be empty)
        RuleFor(b => b.Id)
            .NotEmpty().WithMessage("Id is required.");

        // Validate ApplicationId (should not be empty)
        RuleFor(b => b.ApplicationId)
            .NotEmpty().WithMessage("ApplicationId is required.");

        // Validate InterviewerId (should not be empty)
        RuleFor(b => b.InterviewerId)
            .NotEmpty().WithMessage("InterviewerId is required.");

        // Validate InterviewDate (should not be empty)
        RuleFor(b => b.InterviewDate)
            .NotEmpty().WithMessage("InterviewDate is required.");

        // Validate Status (should not be empty and have max length of 50 characters)
        RuleFor(b => b.Status)
            .NotEmpty().WithMessage("Status is required.")
            .MaximumLength(50).WithMessage("Status must not exceed 50 characters.");

        // Validate Notes (maximum length of 1000 characters)
        RuleFor(b => b.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.");

        // Validate MeetingId (should not be empty)
        RuleFor(b => b.MeetingId)
            .NotEmpty().WithMessage("MeetingId is required.");
    }
}
