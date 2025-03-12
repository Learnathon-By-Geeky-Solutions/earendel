using TalentMesh.Framework.Core.Domain;
using System;

namespace TalentMesh.Module.Evaluator.Domain.Extensions
{
    public static class CommonExtensions
    {
        public static bool IsDeletedOrNotFound<T>(this T? entity) where T : AuditableEntity
        {
            return entity == null || entity.DeletedBy != Guid.Empty;
        }
    }
}
