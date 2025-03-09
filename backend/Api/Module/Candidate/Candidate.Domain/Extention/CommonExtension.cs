
namespace TalentMesh.Module.Candidate.Domain.Extentsion
{
    public static class CommonExtension
    {
        public static bool IsDeletedOrNotFound(this CandidateProfile? entity)
        {
            return entity == null || entity.DeletedBy != Guid.Empty;
        }

        

    }
}

