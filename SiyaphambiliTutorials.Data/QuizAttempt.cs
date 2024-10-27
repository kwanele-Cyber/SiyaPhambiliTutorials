using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents a student's attempt at a quiz.
    /// </summary>
    public class QuizAttempt
    {
        [Key]
        public int QuizAttemptId { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public string StudentId { get; set; }
        public Student Student { get; set; }

        public DateTime AttemptDate { get; set; } = DateTime.UtcNow;

        public int Score { get; set; }

        public ICollection<QuizAnswer> Answers { get; set; }
    }
}
