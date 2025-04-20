namespace TalentMesh.Module.InterviewerView
{
    public record InterviewFeedbackDto(Guid Id, Guid InterviewId, Guid InterviewQuestionId, string Response, decimal Score);
}
