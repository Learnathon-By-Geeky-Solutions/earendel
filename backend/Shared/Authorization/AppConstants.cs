using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Shared.Authorization;
[ExcludeFromCodeCoverage]

public static class AppConstants
{
    public static readonly Collection<string> SupportedImageFormats =
    [
        ".jpeg",
        ".jpg",
        ".png"
    ];
    public static readonly string StandardImageFormat = "image/jpeg";
    public static readonly int MaxImageWidth = 1500;
    public static readonly int MaxImageHeight = 1500;
    public static readonly long MaxAllowedSize = 1000000; // Allows Max File Size of 1 Mb.
}
