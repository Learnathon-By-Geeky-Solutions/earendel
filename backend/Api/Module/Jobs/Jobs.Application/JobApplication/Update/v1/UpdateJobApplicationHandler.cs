using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Job.Application.JobApplication.Update.v1;

namespace TalentMesh.Module.Job.Application.JobApplication.Update.v1
{
    public sealed class UpdateJobApplicationHandler(
        ILogger<UpdateJobApplicationHandler> logger,
        [FromKeyedServices("jobs:jobapplication")] IRepository<Domain.JobApplication> repository)
        : IRequestHandler<UpdateJobApplicationCommand, UpdateJobApplicationResponse>
    {
        public async Task<UpdateJobApplicationResponse> Handle(UpdateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var jobApplication = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (jobApplication == null || jobApplication.DeletedBy != Guid.Empty) throw new JobApplicationNotFoundException(request.Id);


            var updatedJobApplication = jobApplication.Update(request.Status, request.CoverLetter);
            await repository.UpdateAsync(updatedJobApplication, cancellationToken);

            logger.LogInformation("Job application with id : {JobApplicationId} updated.", jobApplication.Id);
            return new UpdateJobApplicationResponse(jobApplication.Id);
        }
    }
}
