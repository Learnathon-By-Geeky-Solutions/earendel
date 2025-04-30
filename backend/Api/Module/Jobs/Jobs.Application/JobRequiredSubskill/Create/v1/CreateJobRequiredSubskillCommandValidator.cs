using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Create.v1
{
    [ExcludeFromCodeCoverage]
    public class CreateJobRequiredSubskillCommandValidator : AbstractValidator<CreateJobRequiredSubskillCommand>
    {
        public CreateJobRequiredSubskillCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();
            RuleFor(x => x.SubskillId).NotEmpty();
        }
    }
}
