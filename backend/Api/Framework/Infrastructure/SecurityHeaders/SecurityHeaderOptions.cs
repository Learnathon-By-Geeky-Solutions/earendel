using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.SecurityHeaders;
[ExcludeFromCodeCoverage]

public class SecurityHeaderOptions
{
    public bool Enable { get; set; }
    public SecurityHeaders Headers { get; set; } = default!;
}
