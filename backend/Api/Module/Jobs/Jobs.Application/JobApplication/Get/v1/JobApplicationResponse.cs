namespace TalentMesh.Module.Job.Application.JobApplication.Get.v1;
public sealed record JobApplicationResponse(
    Guid? Id, Guid JobId ,Guid CandidateId, DateTime ApplicationDate, string Status, string? CoverLetter  
    );
