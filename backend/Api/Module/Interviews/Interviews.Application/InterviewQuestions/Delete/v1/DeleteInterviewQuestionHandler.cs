using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Module.Interviews.Domain.Exceptions; // Ensure this exception exists for InterviewQuestion
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Delete.v1;

public sealed class DeleteInterviewQuestionHandler(
    ILogger<DeleteInterviewQuestionHandler> logger,
    [FromKeyedServices("interviewquestions:interviewquestion")] IRepository<InterviewQuestion> repository)
    : IRequestHandler<DeleteInterviewQuestionCommand>
{
    public async Task Handle(DeleteInterviewQuestionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var interviewQuestion = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (interviewQuestion == null)
            throw new InterviewQuestionNotFoundException(request.Id); // Use the correct exception for InterviewQuestion

        await repository.DeleteAsync(interviewQuestion, cancellationToken);
        logger.LogInformation("InterviewQuestion with ID: {InterviewQuestionId} deleted", interviewQuestion.Id);
    }
}
