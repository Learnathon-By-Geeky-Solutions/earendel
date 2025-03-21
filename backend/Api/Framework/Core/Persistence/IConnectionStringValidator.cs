using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Persistence;
public interface IConnectionStringValidator
{
    bool TryValidate(string connectionString, string? dbProvider = null);
}
