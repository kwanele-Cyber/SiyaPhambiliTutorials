using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
   /// <summary>
    /// Represents an answer to a quiz question in a quiz attempt.
    /// </summary>
    public class QuizAnswer
    {
        [Key]
        public int QuizAnswerId { get; set; }

        public int QuizAttemptId { get; set; }
        public QuizAttempt QuizAttempt { get; set; }

        public int QuizQuestionId { get; set; }
        public QuizQuestion QuizQuestion { get; set; }

        public string AnswerText { get; set; }

        public bool IsCorrect { get; set; }
    }
}
