
namespace TalentMesh.Module.Candidate.Domain.Extention
{
    public static class CommonExtention
    {
        public static bool IsDeletedOrNotFound(this CandidateProfile? entity)
        {
            return entity == null || entity.DeletedBy != Guid.Empty;
        }

        

    }
}
}
