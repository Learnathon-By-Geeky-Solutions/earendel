using FluentValidation;

namespace TalentMesh.Module.Experties.Application.Seniorities.Update.v1;
public class UpdateSeniorityCommandValidator : AbstractValidator<UpdateSeniorityCommand>
{
    public UpdateSeniorityCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Description).MaximumLength(1000);
    }
}
