using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Domain.Extentsion
{
    [ExcludeFromCodeCoverage]
    public static class CommonExtension
    {
        public static bool IsDeletedOrNotFound(this CandidateProfile? entity)
        {
            return entity == null || entity.DeletedBy != Guid.Empty;
        }

        

    }
}

