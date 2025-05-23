using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Update.v1;

public sealed record UpdateInterviewFeedbackCommand(
    Guid Id,                   // Interview Feedback ID
    Guid InterviewId,          // Associated Interview ID
    string InterviewQuestionText, 
    string Response,           // Response to the interview question
    decimal Score                  // Score given for the response
) : IRequest<UpdateInterviewFeedbackResponse>;
