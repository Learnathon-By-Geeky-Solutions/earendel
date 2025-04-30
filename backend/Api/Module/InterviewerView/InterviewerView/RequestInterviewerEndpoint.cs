using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Module.Evaluator.Application.Interviewer;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.InterviewerView
{
    public class RequestInterviewerRequest
    {
        [FromBody]
        public Guid UserId { get; set; }
        [FromBody]
        public string? AdditionalInfo { get; set; }
    }

    [ExcludeFromCodeCoverage]
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
               .WithTags("InterviewerFileHandler")
               .WithName("RequestInterviewer")
               .RequirePermission("Permissions.InterviewerEntryForms.Create")
               .Produces<Guid>(StatusCodes.Status201Created)
               .ProducesValidationProblem()
               .Produces(StatusCodes.Status500InternalServerError);


            return app;
        }
    }
}
