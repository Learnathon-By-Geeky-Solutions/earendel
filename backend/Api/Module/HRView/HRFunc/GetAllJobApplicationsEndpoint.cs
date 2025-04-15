using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For [FromQuery]
using Microsoft.AspNetCore.Routing;


namespace TalentMesh.Module.HRView.HRFunc // Or your preferred namespace
{
    public static class GetAllJobApplicationsEndpoint
    {
        public static RouteHandlerBuilder MapGetAllJobApplicationsEndpoint(this IEndpointRouteBuilder app)
        {
            // GET endpoint to retrieve all job applications
            return app.MapGet("/job-applications", // Using the same base route as the filtered GET
                async (
                    IMediator mediator,
                    [FromQuery] int pageNumber = 1, // Optional query param for pagination
                    [FromQuery] int pageSize = 20   // Optional query param for pagination
                ) =>
                {
                    var query = new GetAllJobApplicationsQuery(pageNumber, pageSize);
                    return await mediator.Send(query);
                })
                .WithTags("JobApplications") // Consistent tagging
                .WithName("GetAllJobApplications")
                .Produces<List<JobApplicationDto>>(StatusCodes.Status200OK) // Specify DTO list as response
                                                                            // Add ProducesResponseType for paginated result if using that structure
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}