namespace TalentMesh.Module.InterviewerView
{
    public record InterviewFeedbackDto(Guid Id, Guid InterviewId, string InterviewQuestionText, string Response, decimal Score);
}
