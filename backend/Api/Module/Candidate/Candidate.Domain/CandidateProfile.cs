using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Module.Candidate.Domain.Events;


namespace TalentMesh.Module.Candidate.Domain;

public class CandidateProfile : AuditableEntity, IAggregateRoot
{

    // Candidate-specific properties.
    public string? Resume { get; set; }
    public string? Skills { get; set; }
    public string? Experience { get; set; }
    public string? Education { get; set; }

    // Reference to the user via a UserId (kept as a Guid here).
    public Guid UserId { get; set; }

    /// <summary>
    /// Factory method to create a CandidateProfile.
    /// </summary>
    /// <param name="candidateId">Unique identifier for the candidate profile.</param>
    /// <param name="userId">Unique identifier for the user.</param>
    /// <param name="resume">Resume content.</param>
    /// <param name="skills">Skills description.</param>
    /// <param name="experience">Experience details.</param>
    /// <param name="education">Education details.</param>
    /// <returns>A new instance of CandidateProfile.</returns>
    public static CandidateProfile Create(
        Guid userId,
        string? resume,
        string? skills,
        string? experience,
        string? education)
    {
        var candidateProfile = new CandidateProfile
        {
            UserId = userId,
            Resume = resume,
            Skills = skills,
            Experience = experience,
            Education = education
        };

        // Queue a domain event to indicate that a candidate profile was created.
        candidateProfile.QueueDomainEvent(new CandidateProfileCreated { CandidateProfile = candidateProfile });
        return candidateProfile;
    }

    /// <summary>
    /// Updates the CandidateProfile properties.
    /// </summary>
    /// <param name="resume">New resume content (optional).</param>
    /// <param name="skills">New skills content (optional).</param>
    /// <param name="experience">New experience details (optional).</param>
    /// <param name="education">New education details (optional).</param>
    /// <returns>The updated CandidateProfile.</returns>
    public CandidateProfile Update(
        string? resume,
        string? skills,
        string? experience,
        string? education)
    {
        if (resume is not null && !string.Equals(Resume, resume, StringComparison.OrdinalIgnoreCase))
            Resume = resume;

        if (skills is not null && !string.Equals(Skills, skills, StringComparison.OrdinalIgnoreCase))
            Skills = skills;

        if (experience is not null && !string.Equals(Experience, experience, StringComparison.OrdinalIgnoreCase))
            Experience = experience;

        if (education is not null && !string.Equals(Education, education, StringComparison.OrdinalIgnoreCase))
            Education = education;

        // Queue a domain event to indicate that the candidate profile was updated.
        this.QueueDomainEvent(new CandidateProfileUpdated { CandidateProfile = this });
        return this;
    }
}


