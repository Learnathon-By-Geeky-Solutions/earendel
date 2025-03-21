namespace TalentMesh.Shared.Authorization;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]

public static class TMClaims
{
    public const string Tenant = "tenant";
    public const string Fullname = "fullName";
    public const string Permission = "permission";
    public const string ImageUrl = "image_url";
    public const string IpAddress = "ipAddress";
    public const string Expiration = "exp";
}
