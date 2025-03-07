using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Create.v1;

public sealed class CreateInterviewQuestionHandler(
    ILogger<CreateInterviewQuestionHandler> logger,
    [FromKeyedServices("interviewquestions:interviewquestion")] IRepository<InterviewQuestion> repository)
    : IRequestHandler<CreateInterviewQuestionCommand, CreateInterviewQuestionResponse>
{
    public async Task<CreateInterviewQuestionResponse> Handle(CreateInterviewQuestionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Create InterviewQuestion using only RubricId and QuestionText
        var interviewQuestion = InterviewQuestion.Create(
            request.RubricId,
            request.QuestionText
        );

        await repository.AddAsync(interviewQuestion, cancellationToken);
        logger.LogInformation("InterviewQuestion created with ID: {InterviewQuestionId}", interviewQuestion.Id);

        return new CreateInterviewQuestionResponse(interviewQuestion.Id);
    }
}
