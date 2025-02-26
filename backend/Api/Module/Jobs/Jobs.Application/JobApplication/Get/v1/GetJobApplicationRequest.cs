using MediatR;

namespace TalentMesh.Module.Job.Application.JobApplication.Get.v1;
public class GetJobApplicationRequest : IRequest<JobApplicationResponse>
{
    public Guid Id { get; set; }
    public GetJobApplicationRequest(Guid id) => Id = id;

    

}
