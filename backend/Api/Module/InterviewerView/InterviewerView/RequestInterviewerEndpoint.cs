using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Module.Evaluator.Application.Interviewer;

namespace TalentMesh.Module.Evaluator.Infrastructure.Endpoints
{
    public class RequestInterviewerRequest
    {
        [FromBody]
        public Guid UserId { get; set; }
        [FromBody]
        public string? AdditionalInfo { get; set; }
    }

    public static class RequestInterviewerEndpoint
    {
        public static IEndpointRouteBuilder MapRequestInterviewer(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/interviewers/request",
                async ([FromBody] RequestInterviewerRequest reqDto,
                       [FromServices] IMediator mediator) =>
                {
                    var cmd = new RequestInterviewerCommand(reqDto.UserId, reqDto.AdditionalInfo);
                    var newId = await mediator.Send(cmd);
                    return Results.Created($"/api/interviewers/{newId}", new { id = newId });
                })
               .WithTags("RakibMagi")
               .WithName("RequestInterviewer")
               .Produces<Guid>(StatusCodes.Status201Created)
               .ProducesValidationProblem()
               .Produces(StatusCodes.Status500InternalServerError);


            return app;
        }
    }
}
