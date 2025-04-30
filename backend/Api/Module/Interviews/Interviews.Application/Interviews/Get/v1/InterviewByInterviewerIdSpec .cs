using Ardalis.Specification;
using TalentMesh.Module.Interviews.Domain;
using System;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.Interviews.Get.v1
{
    [ExcludeFromCodeCoverage]
    public class InterviewByInterviewerIdSpec : Specification<Interview>
    {
        public InterviewByInterviewerIdSpec(Guid interviewerId)
        {
            Query.Where(x => x.InterviewerId == interviewerId);
        }
    }
}
