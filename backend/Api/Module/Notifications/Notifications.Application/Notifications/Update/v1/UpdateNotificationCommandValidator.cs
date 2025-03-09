using FluentValidation;

namespace TalentMesh.Module.Notifications.Application.Notifications.Update.v1;

public class UpdateNotificationCommandValidator : AbstractValidator<UpdateNotificationCommand>
{
    public UpdateNotificationCommandValidator()
    {
        // Validate Entity (Name)
        RuleFor(b => b.Entity)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        // Validate EntityType
        RuleFor(b => b.EntityType)
            .MaximumLength(100);

        // Validate Message
        RuleFor(b => b.Message)
            .MaximumLength(1000);
    }
}
