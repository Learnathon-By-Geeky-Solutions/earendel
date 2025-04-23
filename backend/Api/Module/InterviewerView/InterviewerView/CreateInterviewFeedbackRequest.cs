using Microsoft.AspNetCore.Mvc;

namespace TalentMesh.Module.InterviewerView
{
    // DTOs / Request Models
    public class CreateInterviewFeedbackRequest
    {
        [FromBody]
        public Guid InterviewId { get; set; }
        [FromBody]
        public Guid InterviewQuestionId { get; set; }
        [FromBody]
        public string Response { get; set; } = null!;
        [FromBody]
        public decimal Score { get; set; }
    }
}
