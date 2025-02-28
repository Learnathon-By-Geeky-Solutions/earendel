using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Quizzes.Domain.Events;

namespace TalentMesh.Module.Quizzes.Domain
{
    public class QuizQuestion : AuditableEntity, IAggregateRoot
    {
        public string? QuestionText { get; private set; }
        public string? Option1 { get; private set; }
        public string? Option2 { get; private set; }
        public string? Option3 { get; private set; }
        public string? Option4 { get; private set; }
        public int CorrectOption { get; private set; }

        public static QuizQuestion Create(string questionText, string option1, string option2, string option3, string option4, int correctOption)
        {
            var quizQuestion = new QuizQuestion
            {
                QuestionText = questionText,
                Option1 = option1,
                Option2 = option2,
                Option3 = option3,
                Option4 = option4,
                CorrectOption = correctOption
            };

            quizQuestion.QueueDomainEvent(new QuizQuestionCreated { QuizQuestion = quizQuestion });

            return quizQuestion;
        }

        public QuizQuestion Update(string? questionText, string? option1, string? option2, string? option3, string? option4, int? correctOption)
        {
            if (!string.IsNullOrEmpty(questionText) && questionText != QuestionText)
            {
                QuestionText = questionText;
            }

            if (!string.IsNullOrEmpty(option1) && option1 != Option1)
            {
                Option1 = option1;
            }

            if (!string.IsNullOrEmpty(option2) && option2 != Option2)
            {
                Option2 = option2;
            }

            if (!string.IsNullOrEmpty(option3) && option3 != Option3)
            {
                Option3 = option3;
            }

            if (!string.IsNullOrEmpty(option4) && option4 != Option4)
            {
                Option4 = option4;
            }

            if (correctOption.HasValue && correctOption >= 1 && correctOption <= 4 && correctOption != CorrectOption)
            {
                CorrectOption = correctOption.Value;
            }
            this.QueueDomainEvent(new QuizQuestionUpdated { QuizQuestion = this });

            return this;
        }

        public static QuizQuestion Update(string? questionText, Guid id, string? option1, string? option2, string? option3, string? option4, int correctOption)
        {
            var QuizQuestion = new QuizQuestion
            {
                Id = id,
                QuestionText = questionText,
                Option1 = option1,
                Option2 = option2,
                Option3 = option3,
                Option4 = option4,
                CorrectOption = correctOption
                
            };

            QuizQuestion.QueueDomainEvent(new QuizQuestionUpdated() { QuizQuestion = QuizQuestion });

            return QuizQuestion;
        }
    }
}
