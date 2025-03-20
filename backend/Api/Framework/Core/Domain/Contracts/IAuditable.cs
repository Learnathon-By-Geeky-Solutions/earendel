using System.Diagnostics.CodeAnalysis;
namespace TalentMesh.Framework.Core.Domain.Contracts;

public interface IAuditable
{
    DateTimeOffset Created { get; }
    Guid CreatedBy { get; }
    DateTimeOffset LastModified { get; }
    Guid? LastModifiedBy { get; }
}
