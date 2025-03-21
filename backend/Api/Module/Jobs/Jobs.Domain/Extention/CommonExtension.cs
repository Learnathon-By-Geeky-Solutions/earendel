
using TalentMesh.Framework.Core.Domain;

namespace TalentMesh.Module.Job.Domain.Extentsion
{
    public static class CommonExtension
    {
        public static bool IsDeletedOrNotFound(this Jobs? entity){
            return entity == null || entity.DeletedBy != Guid.Empty;
        }

        public static bool IsDeletedOrNotFound(this JobApplication? entity)
        {
            return entity == null || entity.DeletedBy != Guid.Empty;
        }
        public static bool IsDeletedOrNotFound(this JobRequiredSkill? entity)
        {
            return entity == null || entity.DeletedBy != Guid.Empty;
        }

        public static bool IsDeletedOrNotFound(this JobRequiredSubskill? entity)
        {
            return entity == null || entity.DeletedBy != Guid.Empty;
        }

    }
}
