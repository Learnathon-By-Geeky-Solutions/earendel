namespace TalentMesh.Module.Interviews.Domain;

public class InterviewDetails
{
    public Guid ApplicationId { get; set; }
    public Guid InterviewerId { get; set; }
    public Guid CandidateId { get; set; }
    public Guid JobId { get; set; }
    public DateTime InterviewDate { get; set; }
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }
    public string MeetingId { get; set; } = null!;
}
