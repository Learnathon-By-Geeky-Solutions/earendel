using System.Net;

namespace TalentMesh.Framework.Core.Exceptions;
public class TalentMeshException : Exception
{
    public IEnumerable<string> ErrorMessages { get; }

    public HttpStatusCode StatusCode { get; }

    public TalentMeshException(string message, IEnumerable<string> errors, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        ErrorMessages = errors;
        StatusCode = statusCode;
    }

    public TalentMeshException(string message) : base(message)
    {
        ErrorMessages = new List<string>();
    }
}
