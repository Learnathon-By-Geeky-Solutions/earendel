using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; 
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Module.Quizzes.Infrastructure.Persistence;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Get 
{
    public class GetRandomQuizQuestionQueryHandler : IRequestHandler<GetRandomQuizQuestionQuery, IResult>
    {
        private readonly QuizzesDbContext _context;

        public GetRandomQuizQuestionQueryHandler(QuizzesDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(GetRandomQuizQuestionQuery request, CancellationToken cancellationToken)
        {
            // 1. Validate attempt exists 
            var attemptExists = await _context.QuizAttempts.AnyAsync(a => a.Id == request.AttemptId, cancellationToken);
            if (!attemptExists)
            {
                return Results.NotFound(new { Message = "Quiz attempt not found." });
            }

            // 2. Get IDs of questions already answered in this attempt
            var answeredQuestionIds = await _context.QuizAttemptAnswers
                .Where(ans => ans.AttemptId == request.AttemptId)
                .Select(ans => ans.QuestionId)
                .Distinct()
                .ToListAsync(cancellationToken);

            // 3. Get a random unanswered question
            var randomUnansweredQuestion = await _context.QuizQuestions
                .AsNoTracking()
                .Where(q => !answeredQuestionIds.Contains(q.Id)) // Exclude answered questions
                                                                 // Order by new Guid for random selection (SQL Server specific, adjust for other DBs)
                                                                 // For PostgreSQL use: .OrderBy(q => EF.Functions.Random())
                .OrderBy(q => Guid.NewGuid())
                .Select(q => new QuizQuestionDto // Map to DTO
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    Option1 = q.Option1,
                    Option2 = q.Option2,
                    Option3 = q.Option3,
                    Option4 = q.Option4
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (randomUnansweredQuestion == null)
            {
                // Handle case where no unanswered questions are left
                return Results.Ok(new { Message = "No more questions available for this attempt." }); // Or NotFound()
            }

            return Results.Ok(randomUnansweredQuestion);
        }
    }
}