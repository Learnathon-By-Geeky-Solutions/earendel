using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Update.v1;

public sealed class UpdateInterviewQuestionHandler(
    ILogger<UpdateInterviewQuestionHandler> logger,
    [FromKeyedServices("interviewquestions:interviewquestion")] IRepository<InterviewQuestion> repository)
    : IRequestHandler<UpdateInterviewQuestionCommand, UpdateInterviewQuestionResponse>
{
    public async Task<UpdateInterviewQuestionResponse> Handle(UpdateInterviewQuestionCommand request, CancellationToken cancellationToken)
    {
        // Ensure the request is not null
        ArgumentNullException.ThrowIfNull(request);

        // Fetch the existing interviewQuestion from the repository
        var interviewQuestion = await repository.GetByIdAsync(request.Id, cancellationToken);

        // Check if the interview exists
        if (interviewQuestion is null)
        {
            throw new InterviewQuestionNotFoundException(request.Id);
        }

        // Update the interview entity with the new fields
        interviewQuestion.Update(request.RubricId, request.InterviewId, request.QuestionText!);

        // Save the updated interview back to the repository
        await repository.UpdateAsync(interviewQuestion, cancellationToken);

        // Log the update action
        logger.LogInformation("InterviewQuestion with id: {InterviewQuestionId} updated.", interviewQuestion.Id);

        // Return a response containing the updated interview's ID
        return new UpdateInterviewQuestionResponse(interviewQuestion.Id);
    }
}
