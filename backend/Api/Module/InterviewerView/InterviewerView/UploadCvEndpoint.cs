using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Module.Evaluator.Application.Interviewer;
using TalentMesh.Module.Evaluator.Domain;
using Carter.OpenApi;
namespace TalentMesh.Module.Evaluator.Infrastructure.Endpoints
{
    // DTO to describe the form payload for Swagger
    public class UploadCvRequest
    {
        [FromForm(Name = "cv")]
        public IFormFile Cv { get; set; } = default!;
    }

    public static class UploadCvEndpoint
    {
        public static IEndpointRouteBuilder MapUploadCv(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/interviewers/{id:guid}/upload-cv",
                async (
                    [FromRoute] Guid id,
                    [FromForm(Name = "cv")] IFormFile cv,
                    [FromServices] IMediator mediator) =>
                {
                    if (cv == null || cv.Length == 0) { return Results.BadRequest("CV file is required."); }
                    if (cv.ContentType != "application/pdf") { return Results.BadRequest("Only PDF files are allowed."); }
                    // var updatedForm = await mediator.Send(new UploadCvCommand(id, cv));
                    // Return simple result based on Minimal API inference
                    return Results.Ok($"File received for ID {id}");
                });
            // REMOVED: .WithTags(...).WithName(...).Produces(...)...

            return app;
        }
    }
}