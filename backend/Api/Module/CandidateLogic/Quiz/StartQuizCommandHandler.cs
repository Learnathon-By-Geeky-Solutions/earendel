using MediatR;
using Microsoft.AspNetCore.Http; // For IResult and Results
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Module.Quizzes.Domain;
using TalentMesh.Module.Quizzes.Infrastructure.Persistence;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Start // Or your preferred namespace
{
    [ExcludeFromCodeCoverage]
    public class StartQuizCommandHandler : IRequestHandler<StartQuizCommand, IResult>
    {
        private readonly QuizzesDbContext _context;

        public StartQuizCommandHandler(QuizzesDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(StartQuizCommand request, CancellationToken cancellationToken)
        {
            // Create a new attempt with initial values
            // Assuming 0 score and 0 total questions initially
            // Modify QuizAttempt.Create if needed or use constructor/factory
            var newAttempt = QuizAttempt.Create(
                userId: request.UserId,
                score: 0,          // Initial score
                totalQuestions: 0 // Initially 0, or fetch count if quiz structure is fixed
            );

            _context.QuizAttempts.Add(newAttempt);
            await _context.SaveChangesAsync(cancellationToken);

            // Return the ID of the newly created attempt
            return Results.Ok(new { AttemptId = newAttempt.Id });
        }
    }
}