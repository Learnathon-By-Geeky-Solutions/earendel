using MediatR;

namespace TalentMesh.Module.Job.Application.Jobs.Get.v1;
public class GetJobRequest : IRequest<JobResponse>
{
    public Guid Id { get; set; }
    public GetJobRequest(Guid id) => Id = id;
}
