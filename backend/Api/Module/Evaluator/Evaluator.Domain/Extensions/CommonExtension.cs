using TalentMesh.Framework.Core.Domain;
using System;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Domain.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class CommonExtensions
    {
        public static bool IsDeletedOrNotFound<T>(this T? entity) where T : AuditableEntity
        {
            return entity == null || entity.DeletedBy != Guid.Empty;
        }
    }
}
