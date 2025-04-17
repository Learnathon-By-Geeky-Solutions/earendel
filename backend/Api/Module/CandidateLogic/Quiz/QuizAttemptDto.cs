using System;
using System.Collections.Generic;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Get
{
    // DTO for individual answers within an attempt
    public class QuizAttemptAnswerDto
    {
        public Guid Id { get; set; } // Answer ID
        public Guid QuestionId { get; set; }
        public int SelectedOption { get; set; }
        public bool IsCorrect { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        
    }

    // DTO for the main Quiz Attempt including its answers
    public class QuizAttemptDto
    {
        public Guid Id { get; set; } // Attempt ID
        public Guid UserId { get; set; }
        public decimal Score { get; set; }
        public int TotalQuestions { get; set; }
        public DateTimeOffset CreatedOn { get; set; } // From AuditableEntity
       

        public List<QuizAttemptAnswerDto> Answers { get; set; } = new List<QuizAttemptAnswerDto>();
    }
}