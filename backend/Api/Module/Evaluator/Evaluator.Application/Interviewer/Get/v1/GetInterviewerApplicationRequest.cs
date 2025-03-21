using MediatR;
using System;

namespace Evaluator.Application.Interviewer.Get.v1
{
    public class GetInterviewerApplicationRequest : IRequest<InterviewerApplicationResponse>
    {
        public Guid Id { get; set; }
        public GetInterviewerApplicationRequest(Guid id) => Id = id;
    }
}
