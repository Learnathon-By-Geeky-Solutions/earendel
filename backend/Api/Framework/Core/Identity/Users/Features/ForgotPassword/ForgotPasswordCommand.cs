using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Users.Features.ForgotPassword;
[ExcludeFromCodeCoverage]
public class ForgotPasswordCommand
{
    public string Email { get; set; } = default!;
}
