using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Cors;
using System.Collections.ObjectModel;
[ExcludeFromCodeCoverage]

public class CorsOptions
{
    public CorsOptions()
    {
        AllowedOrigins = [];
    }

    public Collection<string> AllowedOrigins { get; }
}
