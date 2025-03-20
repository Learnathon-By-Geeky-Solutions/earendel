namespace TalentMesh.Framework.Core.Domain.Contracts;
using System.Diagnostics.CodeAnalysis;
public interface ISoftDeletable
{
    DateTimeOffset? Deleted { get; set; }
    Guid? DeletedBy { get; set; }


}
