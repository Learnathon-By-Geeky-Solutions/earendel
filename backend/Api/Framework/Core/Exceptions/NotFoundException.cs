using System.Collections.ObjectModel;
using System.Net;

namespace TalentMesh.Framework.Core.Exceptions;
public class NotFoundException : TalentMeshException
{
    public NotFoundException(string message)
        : base(message, new Collection<string>(), HttpStatusCode.NotFound)
    {
    }
}
