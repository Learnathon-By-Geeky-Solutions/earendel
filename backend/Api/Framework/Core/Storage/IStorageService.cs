using TalentMesh.Framework.Core.Storage.File;
using TalentMesh.Framework.Core.Storage.File.Features;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Storage;
public interface IStorageService
{
    public Task<Uri> UploadAsync<T>(FileUploadCommand? request, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class;

    public void Remove(Uri? path);
}
