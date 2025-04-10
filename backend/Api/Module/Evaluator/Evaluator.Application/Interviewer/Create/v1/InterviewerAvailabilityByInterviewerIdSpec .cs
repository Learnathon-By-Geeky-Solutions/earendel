using Ardalis.Specification;
using TalentMesh.Module.Evaluator.Domain;
using System;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    public class InterviewerAvailabilityByInterviewerIdSpec : Specification<InterviewerAvailability>
    {
        public InterviewerAvailabilityByInterviewerIdSpec(Guid interviewerId)
        {
            Query.Where(availability => availability.InterviewerId == interviewerId && availability.IsAvailable);
        }
    }
}

