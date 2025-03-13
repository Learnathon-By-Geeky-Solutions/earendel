using MediatR;
using System;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Get.v1
{
    public class GetJobRequiredSubskillRequest : IRequest<JobRequiredSubskillResponse>
    {
        public Guid Id { get; set; }
        public GetJobRequiredSubskillRequest(Guid id) => Id = id;
    }
}
