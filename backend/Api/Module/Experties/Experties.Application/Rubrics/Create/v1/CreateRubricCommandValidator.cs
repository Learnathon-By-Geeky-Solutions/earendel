using FluentValidation;


namespace TalentMesh.Module.Experties.Application.Rubrics.Create.v1;
public class CreateRubricCommandValidator : AbstractValidator<CreateRubricCommand>
{
    public CreateRubricCommandValidator()
    {
        RuleFor(b => b.Title).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.RubricDescription).MaximumLength(1000);
    }
}
