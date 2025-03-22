public sealed record CreateInterviewerAvailabilityResponse(
    List<Guid> AvailabilityIds // Updated from single Guid? to List<Guid>
);