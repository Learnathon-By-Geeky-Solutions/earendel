using FluentValidation;

namespace TalentMesh.Module.Experties.Application.Rubrics.Update.v1;
public class UpdateRubricCommandValidator : AbstractValidator<UpdateRubricCommand>
{
    public UpdateRubricCommandValidator()
    {
        RuleFor(b => b.Title).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.RubricDescription).MaximumLength(1000);
    }
}
