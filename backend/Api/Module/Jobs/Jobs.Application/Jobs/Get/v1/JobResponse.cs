namespace TalentMesh.Module.Job.Application.Jobs.Get.v1;
public sealed record JobResponse(
    Guid? Id, string Name, string? Description, 
    string Requirments,string Location, string JobType,
    string ExperienceLevel, string Salary, Guid PostedById);
