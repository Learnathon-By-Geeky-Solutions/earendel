using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Storage.File.Features;
[ExcludeFromCodeCoverage]
public class FileUploadCommand : IRequest<FileUploadResponse>
{
    public string Name { get; set; } = default!;
    public string Extension { get; set; } = default!;
    public string Data { get; set; } = default!;
}

