using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.Interviews.Create.v1;

[ExcludeFromCodeCoverage]
public class CreateInterviewSignatureCommandValidator : AbstractValidator<CreateInterviewSignatureCommand>
{
    public CreateInterviewSignatureCommandValidator()
    {
        RuleFor(b => b.MeetingNumber)
            .NotEmpty().WithMessage("MeetingNumber is required.")
            .Matches(@"^\d+$").WithMessage("MeetingNumber must be a numeric string.");

        RuleFor(b => b.Role)
            .InclusiveBetween(0, 1).WithMessage("Role must be either 0 (attendee) or 1 (host).");
    }
}
