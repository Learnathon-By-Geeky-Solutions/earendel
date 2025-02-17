using FluentValidation;

namespace TalentMesh.Module.Job.Application.Jobs.Update.v1;
public class UpdateBrandCommandValidator : AbstractValidator<UpdateJobCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Description).MaximumLength(1000);
    }
}
