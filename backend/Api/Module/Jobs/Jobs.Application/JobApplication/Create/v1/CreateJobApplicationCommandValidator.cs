using FluentValidation;


namespace TalentMesh.Module.Job.Application.JobApplication.Create.v1;
public class CreateJobApplicationCommandValidator : AbstractValidator<CreateJobApplicationCommand>
{
    public CreateJobApplicationCommandValidator()
    {
        RuleFor(b => b.CandidateId).NotEmpty();
        RuleFor(b => b.JobId).NotEmpty();
        RuleFor(b => b.CoverLetter).MaximumLength(500);

    }
}
