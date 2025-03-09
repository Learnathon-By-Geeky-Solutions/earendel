using FluentValidation;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Update.v1
{
    public class UpdateCandidateProfileCommandValidator : AbstractValidator<UpdateCandidateProfileCommand>
    {
        public UpdateCandidateProfileCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("CandidateProfile Id is required.");
            
        }
    }
}
