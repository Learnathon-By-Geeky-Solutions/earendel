using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For [FromQuery]
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Get; // Namespace for DTO
using TalentMesh.Module.Quizzes.Application.QuizAttempts.GetUserAttempts; // Namespace for Query
using TalentMesh.Framework.Infrastructure.Auth.Policy;

namespace TalentMesh.Module.Quizzes.Api.Endpoints // Or your preferred namespace
{
    public static class GetUserQuizAttemptsEndpoint
    {
        public static RouteHandlerBuilder MapGetUserQuizAttemptsEndpoint(this IEndpointRouteBuilder app)
        {
            // GET endpoint to retrieve all attempts for a specific user
            // Example route: /users/{userId}/quiz-attempts
            return app.MapGet("/users/{userId:guid}/quiz-attempts",
                async (
                    Guid userId, // Get UserId from the route parameter
                    IMediator mediator,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20
                    ) =>
                {
                    // Check if the requesting user is authorized to view this user's attempts
                    // (e.g., is it the same user, or an admin?) - Add authorization logic here if needed.

                    var query = new GetUserQuizAttemptsQuery(userId, pageNumber, pageSize);
                    return await mediator.Send(query);
                })
                // Add .RequireAuthorization() if needed based on your security policy
                .WithTags("QuizView")
                .WithName("GetUserQuizAttempts")
                .RequirePermission("Permissions.Seniorities.View")   // ← view permission
                .Produces<List<QuizAttemptDto>>(StatusCodes.Status200OK) // Specify DTO list
                .Produces(StatusCodes.Status404NotFound) // If user or attempts not found (can be handled in handler)
                .Produces(StatusCodes.Status401Unauthorized) // If authorization fails
                .Produces(StatusCodes.Status403Forbidden)     // If authorized but not permitted
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}