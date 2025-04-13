using System;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Get // Or your preferred namespace
{
    // DTO to avoid exposing the full domain entity
    public class QuizQuestionDto
    {
        public Guid Id { get; set; }
        public string? QuestionText { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        // NOTE: CorrectOption is intentionally omitted from the DTO sent to the client
    }
}