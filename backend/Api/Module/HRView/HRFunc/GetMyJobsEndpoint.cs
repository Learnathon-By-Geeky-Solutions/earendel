using Hangfire.Storage;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For [FromQuery]
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using TalentMesh.Framework.Infrastructure.Auth.Policy;

namespace TalentMesh.Module.HRView.HRFunc
{
    public static class GetMyJobsEndpoint
    {
        public static RouteHandlerBuilder MapGetMyJobsEndpoint(this IEndpointRouteBuilder app)
        {
            // GET endpoint - Requires Authentication
            return app.MapGet("/jobs/my-postings",
                async (
                    IMediator mediator,
                    HttpContext httpContext,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20) =>
                {
                    // Get User ID from claims
                    var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!Guid.TryParse(userIdClaim, out Guid requestingUserId))
                    {
                        // If user ID claim is missing or invalid, return Unauthorized
                        return Results.Unauthorized();
                    }

                    var query = new GetMyJobsQuery(requestingUserId, pageNumber, pageSize);
                    return await mediator.Send(query);
                })
                .RequireAuthorization() // Ensure the user is authenticated
                .WithTags("HRView")
            .WithName("GetMyJobs")
                .Produces<List<JobDto>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status400BadRequest)
                .RequirePermission("Permissions.Jobs.View")
                .Produces(StatusCodes.Status500InternalServerError);

        }
    }
}