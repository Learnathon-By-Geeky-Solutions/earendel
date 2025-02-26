namespace TalentMesh.Module.Job.Application.JobApplication.Get.v1;
public sealed record JobApplicationResponse(
    Guid? Id, int JobId, int CandidateId, DateTime ApplicationDate, string Status, string? CoverLetter  
    );
