using MediatR;

namespace TalentMesh.Module.Job.Application.Jobs.Get.v1;
public class GetBrandRequest : IRequest<UserResponse>
{
    public Guid Id { get; set; }
    public GetBrandRequest(Guid id) => Id = id;
}
