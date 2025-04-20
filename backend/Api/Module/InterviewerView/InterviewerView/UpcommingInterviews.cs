using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;

namespace TalentMesh.Module.InterviewerView
{
    // DTO for Interview entity
    public record InterviewDto(Guid Id, Guid ApplicationId, Guid InterviewerId, DateTime InterviewDate, string Status, string Notes, string MeetingId);

    // Queries
    public record GetUpcomingInterviewsForCandidateQuery(Guid CandidateId) : IRequest<List<InterviewDto>>;
    public record GetUpcomingInterviewsForInterviewerQuery(Guid InterviewerId) : IRequest<List<InterviewDto>>;

    // Handlers
    public class GetUpcomingForCandidateHandler : IRequestHandler<GetUpcomingInterviewsForCandidateQuery, List<InterviewDto>>
    {
        private readonly IRepository<Interview> _repo;
        public GetUpcomingForCandidateHandler(IRepository<Interview> repo) => _repo = repo;
        public async Task<List<InterviewDto>> Handle(GetUpcomingInterviewsForCandidateQuery req, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var list = await _repo.ListAsync(ct);
            return list.Where(i => i.ApplicationId == req.CandidateId && i.InterviewDate >= now)
                       .Select(i => new InterviewDto(i.Id, i.ApplicationId, i.InterviewerId, i.InterviewDate, i.Status, i.Notes, i.MeetingId))
                       .ToList();
        }
    }

    public class GetUpcomingForInterviewerHandler : IRequestHandler<GetUpcomingInterviewsForInterviewerQuery, List<InterviewDto>>
    {
        private readonly IRepository<Interview> _repo;
        public GetUpcomingForInterviewerHandler(IRepository<Interview> repo) => _repo = repo;
        public async Task<List<InterviewDto>> Handle(GetUpcomingInterviewsForInterviewerQuery req, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var list = await _repo.ListAsync(ct);
            return list.Where(i => i.InterviewerId == req.InterviewerId && i.InterviewDate >= now)
                       .Select(i => new InterviewDto(i.Id, i.ApplicationId, i.InterviewerId, i.InterviewDate, i.Status, i.Notes, i.MeetingId))
                       .ToList();
        }
    }

    // Endpoints
    public static class InterviewUpcomingEndpoint
    {
        public static IEndpointRouteBuilder MapUpcomingInterviews(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/interviews/upcoming/candidate/{candidateId:guid}", async (
                    [FromRoute] Guid candidateId,
                    [FromServices] IMediator mediator) =>
            {
                var dtos = await mediator.Send(new GetUpcomingInterviewsForCandidateQuery(candidateId));
                return Results.Ok(dtos);
            })
               .WithName("GetUpcomingForCandidate")
               .WithTags("UpcommingInterview")
               .Produces<List<InterviewDto>>(StatusCodes.Status200OK);

            app.MapGet("/api/interviews/upcoming/interviewer/{interviewerId:guid}", async (
                    [FromRoute] Guid interviewerId,
                    [FromServices] IMediator mediator) =>
            {
                var dtos = await mediator.Send(new GetUpcomingInterviewsForInterviewerQuery(interviewerId));
                return Results.Ok(dtos);
            })
               .WithName("GetUpcomingForInterviewer")
               .WithTags("UpcommingInterview")
               .Produces<List<InterviewDto>>(StatusCodes.Status200OK);

            return app;
        }
    }
}
