using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Application.Jobs.Update.v1;
[ExcludeFromCodeCoverage]
public class UpdateBrandCommandValidator : AbstractValidator<UpdateJobCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Description).MaximumLength(1000);
        RuleFor(b => b.Requirments).NotEmpty().MaximumLength(1000);
        RuleFor(b => b.Location).NotEmpty().MaximumLength(100);
        RuleFor(b => b.JobType).NotEmpty().MaximumLength(100);
        RuleFor(b => b.ExperienceLevel).NotEmpty().MaximumLength(100);

    }
}
