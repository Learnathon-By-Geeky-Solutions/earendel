using MediatR;
using Microsoft.AspNetCore.Http; // For IResult
using System;
using System.Collections.Generic;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Get; // Namespace for the DTO

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.GetUserAttempts 
{
    // Query to get all attempts for a specific user
    public record GetUserQuizAttemptsQuery(
        Guid UserId,          // The ID of the user whose attempts to fetch
        int PageNumber = 1,
        int PageSize = 20
    ) : IRequest<IResult>; // Returns a list (or paginated list) of QuizAttemptDto
}