using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents a question within a quiz.
    /// </summary>
    public class QuizQuestion
    {
        [Key]
        public int QuizQuestionId { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        public string QuestionType { get; set; } // e.g., MultipleChoice, TrueFalse, ShortAnswer

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public ICollection<QuizOption> Options { get; set; }

        public ICollection<QuizAnswer> QuizeAnswers { get; set; }
    }
}
