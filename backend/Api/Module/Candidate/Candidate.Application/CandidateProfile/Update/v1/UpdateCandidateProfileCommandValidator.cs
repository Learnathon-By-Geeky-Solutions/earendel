using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Update.v1
{
    [ExcludeFromCodeCoverage]
    public class UpdateCandidateProfileCommandValidator : AbstractValidator<UpdateCandidateProfileCommand>
    {
        public UpdateCandidateProfileCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("CandidateProfile Id is required.");
            
        }
    }
}
