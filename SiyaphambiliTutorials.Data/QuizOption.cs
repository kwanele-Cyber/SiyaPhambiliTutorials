using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data{
    /// <summary>
    /// Represents an option for a multiple-choice question.
    /// </summary>
    public class QuizOption
    {
        [Key]
        public int QuizOptionId { get; set; }

        public string OptionText { get; set; }

        public bool IsCorrect { get; set; }

        public int QuizQuestionId { get; set; }
        public QuizQuestion QuizQuestion { get; set; }
    }
}