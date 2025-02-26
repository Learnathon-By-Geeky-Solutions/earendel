using FluentValidation;


namespace TalentMesh.Module.Experties.Application.Seniorities.Create.v1;
public class CreateSeniorityCommandValidator : AbstractValidator<CreateSeniorityCommand>
{
    public CreateSeniorityCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Description).MaximumLength(1000);
    }
}
