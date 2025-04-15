using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For [FromBody]
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.HRView.HRFunc 
{
    // Request body structure matching the command
    public class CreateJobRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Requirments { get; set; } = default!; // Spelling
        public string Location { get; set; } = default!;
        public string JobType { get; set; } = default!;
        public string ExperienceLevel { get; set; } = default!;
        public string? Salary { get; set; }
        public Guid PostedBy { get; set; } 
        public int NumberOfInterviews { get; set; }
        public List<Guid>? RequiredSkillIds { get; set; } = new List<Guid>();
        public List<Guid>? RequiredSubskillIds { get; set; } = new List<Guid>();
    }

    public static class CreateJobEndpoint
    {
        public static RouteHandlerBuilder MapCreateJobEndpoint(this IEndpointRouteBuilder app)
        {
            // POST endpoint to create a new job
            return app.MapPost("/jobs",
                async (
                    [FromBody] CreateJobRequest request, // Get data from request body
                    IMediator mediator) =>
                {
                    // TODO: Add input validation (e.g., using FluentValidation)
                    var command = new CreateJobCommand(
                        Name: request.Name,
                        Description: request.Description,
                        Requirments: request.Requirments, // Spelling
                        Location: request.Location,
                        JobType: request.JobType,
                        ExperienceLevel: request.ExperienceLevel,
                        Salary: request.Salary,
                        PostedBy: request.PostedBy,
                        NumberOfInterviews: request.NumberOfInterviews,
                        RequiredSkillIds: request.RequiredSkillIds ?? new List<Guid>(),
                        RequiredSubskillIds: request.RequiredSubskillIds ?? new List<Guid>()
                    );

                    return await mediator.Send(command);
                })
                .WithTags("HRView") // Group in Swagger
                .WithName("CreateJob")
                .Produces<object>(StatusCodes.Status201Created) // Returns { JobId: "guid" } in body and Location header
                .ProducesValidationProblem() // For potential validation errors
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}