
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Create.v1;

[ExcludeFromCodeCoverage]
public class CreateCandidateProfileCommandValidator : AbstractValidator<CreateCandidateProfileCommand>
{
    public CreateCandidateProfileCommandValidator()
    {
        RuleFor(b => b.Resume).NotEmpty().MaximumLength(500);
        RuleFor(b => b.Skills).NotEmpty().MaximumLength(500);
        RuleFor(b => b.Experience).NotEmpty();
        RuleFor(b => b.Education).NotEmpty();
        
    }
}

