using System.Collections.ObjectModel;
using System.Net;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Exceptions;
[ExcludeFromCodeCoverage]
public class UnauthorizedException : TalentMeshException
{
    public UnauthorizedException()
        : base("authentication failed", new Collection<string>(), HttpStatusCode.Unauthorized)
    {
    }
    public UnauthorizedException(string message)
       : base(message, new Collection<string>(), HttpStatusCode.Unauthorized)
    {
    }
}
