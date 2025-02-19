using System.Collections.ObjectModel;
using System.Net;

namespace TalentMesh.Framework.Core.Exceptions;
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
