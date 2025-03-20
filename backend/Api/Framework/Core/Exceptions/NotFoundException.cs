using System.Collections.ObjectModel;
using System.Net;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Exceptions;
[ExcludeFromCodeCoverage]
public class NotFoundException : TalentMeshException
{
    public NotFoundException(string message)
        : base(message, new Collection<string>(), HttpStatusCode.NotFound)
    {
    }
}
