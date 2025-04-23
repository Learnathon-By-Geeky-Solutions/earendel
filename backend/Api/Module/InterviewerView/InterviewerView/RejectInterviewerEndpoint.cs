using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Module.Evaluator.Application.Interviewer;

namespace TalentMesh.Module.InterviewerView
{
    public static class RejectInterviewerEndpoint
    {
        public static IEndpointRouteBuilder MapRejectInterviewer(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/interviewers/{id:guid}/reject",
                async ([FromRoute] Guid id,
                       [FromServices] IMediator mediator) =>
                {
                    await mediator.Send(new RejectInterviewerCommand(id));
                    return Results.NoContent();
                })
               .WithTags("InterviewerFileHandler")
               .WithName("RejectInterviewer")
               .Produces(StatusCodes.Status204NoContent)
               .Produces(StatusCodes.Status404NotFound)
               .Produces(StatusCodes.Status500InternalServerError);

            return app;
        }
    }
}