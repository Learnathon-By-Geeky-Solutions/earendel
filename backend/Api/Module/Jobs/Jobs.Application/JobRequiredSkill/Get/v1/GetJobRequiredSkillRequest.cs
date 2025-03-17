using MediatR;
using System;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Get.v1
{
    public class GetJobRequiredSkillRequest : IRequest<JobRequiredSkillResponse>
    {
        public Guid Id { get; set; }
        public GetJobRequiredSkillRequest(Guid id) => Id = id;
    }
}
