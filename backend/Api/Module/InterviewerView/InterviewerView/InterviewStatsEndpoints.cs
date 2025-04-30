
using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;

namespace TalentMesh.Module.InterviewerView
{
    [ExcludeFromCodeCoverage]
    // Endpoints
    public static class InterviewFeedbackEndpoint
    {
        public static IEndpointRouteBuilder MapInterviewFeedback(this IEndpointRouteBuilder app)
        {
            // GET all feedback
            app.MapGet("/api/feedbacks", async ([FromServices] IMediator mediator) =>
            {
                var dtos = await mediator.Send(new GetAllInterviewFeedbackQuery());
                return Results.Ok(dtos);
            })
            .WithName("GetAllInterviewFeedback")
            .WithTags("InterviewerReportGenerator")
            .Produces<List<InterviewFeedbackDto>>(StatusCodes.Status200OK);

            // POST new feedback
            app.MapPost("/api/feedbacks", async (
                    [FromBody] CreateInterviewFeedbackRequest req,
                    [FromServices] IMediator mediator) =>
            {
                var cmd = new CreateInterviewFeedbackCommand(
                    req.InterviewId,
                    req.InterviewQuestionText,
                    req.Response,
                    req.Score);

                var dto = await mediator.Send(cmd);
                return Results.Created($"/api/feedbacks/{dto.Id}", dto);
            })
            .WithName("CreateInterviewFeedback")
            .WithTags("InterviewerReportGenerator")
            .Produces<InterviewFeedbackDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status500InternalServerError);

            // GET feedback report
            app.MapGet("/api/feedbacks/report", async ([FromServices] IMediator mediator) =>
            {
                var report = await mediator.Send(new GetInterviewFeedbackReportQuery());
                return Results.Ok(report);
            })
            .WithName("GetInterviewFeedbackReport")
            .WithTags("InterviewerReportGenerator")
            .Produces<List<InterviewFeedbackReportDto>>(StatusCodes.Status200OK);

            return app;
        }
    }

    // Queries & Commands + Handlers
    public record GetAllInterviewFeedbackQuery() : IRequest<List<InterviewFeedbackDto>>;
    public class GetAllInterviewFeedbackHandler : IRequestHandler<GetAllInterviewFeedbackQuery, List<InterviewFeedbackDto>>
    {
        private readonly IRepository<InterviewFeedback> _repo;
        public GetAllInterviewFeedbackHandler(IRepository<InterviewFeedback> repo) => _repo = repo;
        public async Task<List<InterviewFeedbackDto>> Handle(GetAllInterviewFeedbackQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repo.ListAsync(cancellationToken);
            return entities.Select(x => new InterviewFeedbackDto(x.Id, x.InterviewId, x.InterviewQuestionText, x.Response, x.Score)).ToList();
        }
    }

    public record CreateInterviewFeedbackCommand(Guid InterviewId, string InterviewQuestionText, string Response, decimal Score) : IRequest<InterviewFeedbackDto>;
    public class CreateInterviewFeedbackHandler : IRequestHandler<CreateInterviewFeedbackCommand, InterviewFeedbackDto>
    {
        private readonly IRepository<InterviewFeedback> _repo;
        public CreateInterviewFeedbackHandler(IRepository<InterviewFeedback> repo) => _repo = repo;
        public async Task<InterviewFeedbackDto> Handle(CreateInterviewFeedbackCommand request, CancellationToken cancellationToken)
        {
            var entity = InterviewFeedback.Create(request.InterviewId, request.InterviewQuestionText, request.Response, request.Score);
            await _repo.AddAsync(entity, cancellationToken);
            return new InterviewFeedbackDto(entity.Id, entity.InterviewId, entity.InterviewQuestionText, entity.Response, entity.Score);
        }
    }

    public record GetInterviewFeedbackReportQuery() : IRequest<List<InterviewFeedbackReportDto>>;
    public class GetInterviewFeedbackReportHandler : IRequestHandler<GetInterviewFeedbackReportQuery, List<InterviewFeedbackReportDto>>
    {
        private readonly IRepository<InterviewFeedback> _repo;
        public GetInterviewFeedbackReportHandler(IRepository<InterviewFeedback> repo) => _repo = repo;
        public async Task<List<InterviewFeedbackReportDto>> Handle(GetInterviewFeedbackReportQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.ListAsync(cancellationToken);
            return list
                .GroupBy(x => x.InterviewId)
                .Select(g => new InterviewFeedbackReportDto(g.Key, g.Average(x => x.Score)))
                .ToList();
        }
    }
}

namespace TalentMesh.Module.InterviewerView
{
    // DTOs / Request Models
    public class CreateInterviewQuestionRequest
    {
        [FromBody]
        public Guid RubricId { get; set; }
        [FromBody]
        public Guid InterviewId { get; set; }
        [FromBody]
        public string QuestionText { get; set; } = null!;
    }

    public record InterviewQuestionDto(Guid Id, Guid RubricId, Guid InterviewId, string QuestionText);

    // Endpoints
    public static class InterviewQuestionEndpoint
    {
        public static IEndpointRouteBuilder MapInterviewQuestion(this IEndpointRouteBuilder app)
        {
            // GET all questions
            app.MapGet("/api/questions", async ([FromServices] IMediator mediator) =>
            {
                var dtos = await mediator.Send(new GetAllInterviewQuestionsQuery());
                return Results.Ok(dtos);
            })
            .WithName("GetAllInterviewQuestions")
            .WithTags("InterviewerReportGenerator")
            .Produces<List<InterviewQuestionDto>>(StatusCodes.Status200OK);

            // POST new question
            app.MapPost("/api/questions", async (
                    [FromBody] CreateInterviewQuestionRequest req,
                    [FromServices] IMediator mediator) =>
            {
                var cmd = new CreateInterviewQuestionCommand(
                    req.RubricId,
                    req.InterviewId,
                    req.QuestionText);

                var dto = await mediator.Send(cmd);
                return Results.Created($"/api/questions/{dto.Id}", dto);
            })
            .WithName("CreateInterviewQuestion")
            .WithTags("InterviewerReportGenerator")
            .Produces<InterviewQuestionDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status500InternalServerError);

            return app;
        }
    }

    // Queries & Commands + Handlers
    public record GetAllInterviewQuestionsQuery() : IRequest<List<InterviewQuestionDto>>;
    public class GetAllInterviewQuestionsHandler : IRequestHandler<GetAllInterviewQuestionsQuery, List<InterviewQuestionDto>>
    {
        private readonly IRepository<InterviewQuestion> _repo;
        public GetAllInterviewQuestionsHandler(IRepository<InterviewQuestion> repo) => _repo = repo;
        public async Task<List<InterviewQuestionDto>> Handle(GetAllInterviewQuestionsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repo.ListAsync(cancellationToken);
            return entities.Select(x => new InterviewQuestionDto(x.Id, x.RubricId, x.InterviewId, x.QuestionText)).ToList();
        }
    }

    public record CreateInterviewQuestionCommand(Guid RubricId, Guid InterviewId, string QuestionText) : IRequest<InterviewQuestionDto>;
    public class CreateInterviewQuestionHandler : IRequestHandler<CreateInterviewQuestionCommand, InterviewQuestionDto>
    {
        private readonly IRepository<InterviewQuestion> _repo;
        public CreateInterviewQuestionHandler(IRepository<InterviewQuestion> repo) => _repo = repo;
        public async Task<InterviewQuestionDto> Handle(CreateInterviewQuestionCommand request, CancellationToken cancellationToken)
        {
            var entity = InterviewQuestion.Create(request.RubricId, request.InterviewId, request.QuestionText);
            await _repo.AddAsync(entity, cancellationToken);
            return new InterviewQuestionDto(entity.Id, entity.RubricId, entity.InterviewId, entity.QuestionText);
        }
    }
}
