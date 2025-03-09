using FluentValidation;

namespace TalentMesh.Module.Notifications.Application.Notifications.Create.v1;

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(b => b.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(b => b.Entity)
            .NotEmpty().WithMessage("Entity is required.")
            .MaximumLength(200).WithMessage("Entity must not exceed 200 characters.");

        RuleFor(b => b.EntityType)
            .MaximumLength(100).WithMessage("EntityType must not exceed 100 characters.");

        RuleFor(b => b.Message)
            .MaximumLength(1000).WithMessage("Message must not exceed 1000 characters.");
    }
}
