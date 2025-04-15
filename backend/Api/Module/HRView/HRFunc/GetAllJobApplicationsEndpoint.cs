using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For [FromQuery]
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;


namespace TalentMesh.Module.HRView.HRFunc // Or your preferred namespace
{
    public static class GetAllJobApplicationsEndpoint
    {
        public static RouteHandlerBuilder MapGetAllJobApplicationsEndpoint(this IEndpointRouteBuilder app)
        {
            // GET endpoint - Requires Authentication
            return app.MapGet("/job-applications/my-postings", // Changed route slightly for clarity
                async (
                    IMediator mediator,
                    HttpContext httpContext,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20) => // Inject HttpContext to access user claims
                {
                    // --- Get User ID ---
                    // Example: Retrieve User ID from claims (adjust claim type as needed)
                    var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); // Or your specific user ID claim type
                    if (!Guid.TryParse(userIdClaim, out Guid requestingUserId))
                    {
                        // If user ID claim is missing or invalid, return Unauthorized or Bad Request
                        return Results.Unauthorized();
                    }
                    // --- ---

                    var query = new GetAllJobApplicationsQuery(requestingUserId, pageNumber, pageSize);
                    return await mediator.Send(query);
                })
                .RequireAuthorization() // Ensure the user is authenticated
                .WithTags("JobApplications")
                .WithName("GetMyJobApplications") // More specific name
                .Produces<List<JobApplicationDto>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized) // Added Unauthorized response
                .Produces(StatusCodes.Status400BadRequest)  // Added Bad Request response
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}