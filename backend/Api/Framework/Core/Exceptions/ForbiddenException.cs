using System.Net;

namespace TalentMesh.Framework.Core.Exceptions;
public class ForbiddenException : TalentMeshException
{
    public ForbiddenException()
        : base("unauthorized", [], HttpStatusCode.Forbidden)
    {
    }
    public ForbiddenException(string message)
       : base(message, [], HttpStatusCode.Forbidden)
    {
    }
}
