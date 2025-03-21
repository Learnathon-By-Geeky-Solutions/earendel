using MediatR;
using System;

namespace Evaluator.Application.Interviewer.Get.v1
{
    public class GetInterviewerAvailabilityRequest : IRequest<InterviewerAvailabilityResponse>
    {
        public Guid Id { get; set; }
        public GetInterviewerAvailabilityRequest(Guid id) => Id = id;
    }
}
