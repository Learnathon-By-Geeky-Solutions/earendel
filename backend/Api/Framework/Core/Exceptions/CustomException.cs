using System.Net;
using System.Diagnostics.CodeAnalysis;
namespace TalentMesh.Framework.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class CustomException : Exception
{
    public List<string>? ErrorMessages { get; }

    public HttpStatusCode StatusCode { get; }

    public CustomException(string message, List<string>? errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        ErrorMessages = errors;
        StatusCode = statusCode;
    }
}
