using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Domain; // Use the correct namespace for JobApplication

namespace TalentMesh.Module.CandidateLogic.JobApplicationView // Or your preferred namespace
{
    public static class JobApplicationViewEndpoint
    {
        public static RouteHandlerBuilder MapJobApplicationViewEndpoints(this IEndpointRouteBuilder app)
        {
            // Define the route, e.g., "/job-applications"
            return app.MapGet("/JobApplicationView",
                async (
                    IMediator mediator,
                    [FromQuery] Guid? jobId,
                    [FromQuery] Guid? candidateId, // User ID
                    [FromQuery] DateTime? applicationDateStart,
                    [FromQuery] DateTime? applicationDateEnd,
                    [FromQuery] string? status) =>
                {
                    var filters = new JobApplicationViewFilters(
                        jobId,
                        candidateId,
                        applicationDateStart,
                        applicationDateEnd,
                        status);

                    // Send the filters to the handler (JobApplicationViewService)
                    try
                    {
                        return await mediator.Send(filters);
                    }
                    catch (Exception ex)
                    {
                        // Log exception
                        return Results.Problem("An error occurred while processing your request.",
                                              statusCode: StatusCodes.Status500InternalServerError);
                    }
                })
                .WithTags("CandidateLogic") // Group in Swagger UI
                .WithName("GetFilteredJobApplications") // Unique name for the endpoint
                .Produces<List<JobApplication>>(StatusCodes.Status200OK) // Specify the expected success response
                //.RequirePermission("Permissions.CandidateLogic.View")
                .Produces(StatusCodes.Status400BadRequest) // Example error response
                .Produces(StatusCodes.Status500InternalServerError); // Example error response
        }
    }
}
