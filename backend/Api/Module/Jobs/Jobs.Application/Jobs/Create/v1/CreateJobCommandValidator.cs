using FluentValidation;


namespace TalentMesh.Module.Job.Application.Jobs.Create.v1;
public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Description).MaximumLength(1000);
        RuleFor(b => b.Requirments).NotEmpty().MinimumLength(2).MaximumLength(1000);
        RuleFor(b => b.Location).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.JobType).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.ExperienceLevel).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Salary).NotEmpty().MinimumLength(2).MaximumLength(100);
    }
}
