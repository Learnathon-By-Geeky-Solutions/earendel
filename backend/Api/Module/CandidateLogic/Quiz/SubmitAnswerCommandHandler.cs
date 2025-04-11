using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Module.Quizzes.Domain;
using TalentMesh.Module.Quizzes.Infrastructure.Persistence;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.SubmitAnswer // Or your preferred namespace
{
    public class SubmitAnswerCommandHandler : IRequestHandler<SubmitAnswerCommand, IResult>
    {
        private readonly QuizzesDbContext _context;

        public SubmitAnswerCommandHandler(QuizzesDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
        {
            // 1. Find the Quiz Attempt
            var quizAttempt = await _context.QuizAttempts
                                    .FirstOrDefaultAsync(a => a.Id == request.AttemptId, cancellationToken);

            if (quizAttempt == null)
            {
                return Results.NotFound(new { Message = "Quiz attempt not found." });
            }

            // 2. Find the Quiz Question
            var quizQuestion = await _context.QuizQuestions
                                     .AsNoTracking() // No need to track for comparison
                                     .FirstOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken);

            if (quizQuestion == null)
            {
                return Results.NotFound(new { Message = "Quiz question not found." });
            }

            // 3. Check if already answered (optional, prevent re-answering)
            var alreadyAnswered = await _context.QuizAttemptAnswers
                                      .AnyAsync(ans => ans.AttemptId == request.AttemptId && ans.QuestionId == request.QuestionId, cancellationToken);
            if (alreadyAnswered)
            {
                return Results.Conflict(new { Message = "This question has already been answered for this attempt." });
            }


            // 4. Determine correctness
            bool isCorrect = quizQuestion.CorrectOption == request.SelectedOption;

            // 5. Create the Answer record
            var newAnswer = QuizAttemptAnswer.Create(
                attemptId: request.AttemptId,
                questionId: request.QuestionId,
                selectedOption: request.SelectedOption,
                isCorrect: isCorrect
            );
            _context.QuizAttemptAnswers.Add(newAnswer);

            // 6. Update the QuizAttempt score
            // We need a way to update the score. The existing Update methods aren't ideal.
            // Let's modify the fetched entity directly (ensure it's tracked).
            // A domain method like quizAttempt.RecordAnswer(isCorrect) would be cleaner.
            decimal newScore = quizAttempt.Score + (isCorrect ? 1 : 0); // Simple scoring: +1 if correct
            int newTotalQuestions = quizAttempt.TotalQuestions + 1; // Increment answered count

            // Use the static Update factory method for consistency or add a dedicated method
            // This replaces the tracked entity with a new one before saving
            var updatedAttempt = QuizAttempt.Update(
                id: quizAttempt.Id,
                userId: quizAttempt.UserId, // Keep original UserId
                score: newScore,
                totalQuestions: newTotalQuestions
                );

            // If using direct modification on the tracked entity (alternative):
            // quizAttempt.GetType().GetProperty("Score").SetValue(quizAttempt, newScore, null); // Reflection needed due to private setters
            // quizAttempt.GetType().GetProperty("TotalQuestions").SetValue(quizAttempt, newTotalQuestions, null);
            // _context.QuizAttempts.Update(quizAttempt); // Mark as modified if needed

            _context.Entry(quizAttempt).CurrentValues.SetValues(updatedAttempt); // Apply changes to tracked entity


            // 7. Save changes
            await _context.SaveChangesAsync(cancellationToken);

            // 8. Return result
            return Results.Ok(new
            {
                AttemptId = request.AttemptId,
                QuestionId = request.QuestionId,
                IsCorrect = isCorrect,
                NewScore = newScore
            });
        }
    }
}