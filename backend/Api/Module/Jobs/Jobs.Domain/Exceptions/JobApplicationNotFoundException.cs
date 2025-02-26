using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Job.Domain.Exceptions
{
    public sealed class JobApplicationNotFoundException : NotFoundException
    {
        public JobApplicationNotFoundException(Guid id)
       : base($"JobApplication with id {id} not found")
        {
        }
    }
}
