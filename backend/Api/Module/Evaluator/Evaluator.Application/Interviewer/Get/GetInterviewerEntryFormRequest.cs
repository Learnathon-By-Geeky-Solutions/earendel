using MediatR;
using System;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Get.v1
{
    public class GetInterviewerEntryFormRequest : IRequest<InterviewerEntryFormResponse>
    {
        public Guid Id { get; set; }
        public GetInterviewerEntryFormRequest(Guid id) => Id = id;
    }
}
