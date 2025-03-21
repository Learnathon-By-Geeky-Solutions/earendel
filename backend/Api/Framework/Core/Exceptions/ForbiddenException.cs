using System.Net;
using System.Diagnostics.CodeAnalysis;
namespace TalentMesh.Framework.Core.Exceptions;
[ExcludeFromCodeCoverage]
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
