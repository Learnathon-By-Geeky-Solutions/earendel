namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Get.v1;

public sealed record InterviewFeedbackResponse(
    Guid Id,                  // Unique identifier for the feedback
    Guid InterviewId,         // Associated interview ID
    Guid InterviewQuestionId, // Question ID related to the feedback
    string Response,          // Candidate's response to the question
    decimal Score             // Score assigned to the response
);
