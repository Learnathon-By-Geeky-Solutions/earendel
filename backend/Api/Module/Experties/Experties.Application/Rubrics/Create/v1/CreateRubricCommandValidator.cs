using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Rubrics.Create.v1;
[ExcludeFromCodeCoverage]
public class CreateRubricCommandValidator : AbstractValidator<CreateRubricCommand>
{
    public CreateRubricCommandValidator()
    {
        RuleFor(b => b.Title).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.RubricDescription).MaximumLength(1000);
    }
}
