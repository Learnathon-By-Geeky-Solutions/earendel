using Ardalis.Specification;
using TalentMesh.Module.Interviews.Domain;
using System;

namespace TalentMesh.Module.Interviews.Application.Interviews.Get.v1
{
    public class InterviewByInterviewerIdSpec : Specification<Interview>
    {
        public InterviewByInterviewerIdSpec(Guid interviewerId)
        {
            Query.Where(x => x.InterviewerId == interviewerId);
        }
    }
}
