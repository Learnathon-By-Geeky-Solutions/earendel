using namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1;

public sealed record CreateInterviewerAvailabilityResponse(
    List<Guid> AvailabilityIds // Updated from single Guid? to List<Guid>
);