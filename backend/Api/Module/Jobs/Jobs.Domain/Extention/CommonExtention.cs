
using TalentMesh.Framework.Core.Domain;

namespace TalentMesh.Module.Job.Domain.Extention
{
    public static class CommonExtention
    {
        public static bool IsDeletedOrNotFound(this Jobs? entity)         {
            return entity == null || entity.DeletedBy != Guid.Empty;
        }

        public static bool IsDeletedOrNotFound(this JobApplication? entity)
        {
            return entity == null || entity.DeletedBy != Guid.Empty;
        }

    }
}
