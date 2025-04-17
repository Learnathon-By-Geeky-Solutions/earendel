using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Get; // Namespace for DTOs
using TalentMesh.Module.Quizzes.Infrastructure.Persistence;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.GetUserAttempts // Or your preferred namespace
{
    public class GetUserQuizAttemptsQueryHandler : IRequestHandler<GetUserQuizAttemptsQuery, IResult>
    {
        private readonly QuizzesDbContext _context;

        public GetUserQuizAttemptsQueryHandler(QuizzesDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(GetUserQuizAttemptsQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
            int pageSize = request.PageSize > 0 ? request.PageSize : 20;

            // --- 1. Filter Quiz Attempts for the specific User ---
            var attemptQuery = _context.QuizAttempts
                .AsNoTracking()
                .Where(a => a.UserId == request.UserId) // Filter by UserId
                .OrderByDescending(a => a.Created); // Example ordering

            // Get total count for pagination
            var totalCount = await attemptQuery.CountAsync(cancellationToken);

            // Select necessary data for attempts and apply pagination
            var userAttempts = await attemptQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new // Project to keep only needed fields initially
                {
                    a.Id,
                    a.UserId,
                    a.Score,
                    a.TotalQuestions,
                    a.Created
                })
                .ToListAsync(cancellationToken);

            if (!userAttempts.Any())
            {
                return Results.Ok(new List<QuizAttemptDto>()); // Return empty list if no attempts found
            }

            var attemptIds = userAttempts.Select(a => a.Id).ToList();

            // --- 2. Fetch Related Answers for these attempts ---
            var relatedAnswers = await _context.QuizAttemptAnswers
                .AsNoTracking()
                .Where(ans => attemptIds.Contains(ans.AttemptId))
                .Select(ans => new // Project to needed fields
                {
                    ans.Id,
                    ans.AttemptId,
                    ans.QuestionId,
                    ans.SelectedOption,
                    ans.IsCorrect,
                    ans.Created
                })
                .ToListAsync(cancellationToken);

            // --- 3. Group Answers by AttemptId ---
            var answersByAttemptId = relatedAnswers.ToLookup(ans => ans.AttemptId);

            // --- 4. Map to DTO ---
            var results = userAttempts.Select(attempt => new QuizAttemptDto
            {
                Id = attempt.Id,
                UserId = attempt.UserId,
                Score = attempt.Score,
                TotalQuestions = attempt.TotalQuestions,
                CreatedOn = attempt.Created,
                Answers = answersByAttemptId.Contains(attempt.Id)
                    ? answersByAttemptId[attempt.Id].Select(ans => new QuizAttemptAnswerDto
                    {
                        Id = ans.Id,
                        QuestionId = ans.QuestionId,
                        SelectedOption = ans.SelectedOption,
                        IsCorrect = ans.IsCorrect,
                        CreatedOn = ans.Created
                    }).ToList()
                    : new List<QuizAttemptAnswerDto>()
            }).ToList();


            // For Pagincation: Return structured paginated result
            // var paginatedResult = new PaginatedResult<QuizAttemptDto>(results, totalCount, pageNumber, pageSize);
            // return Results.Ok(paginatedResult);

            return Results.Ok(results);
        }
    }
}