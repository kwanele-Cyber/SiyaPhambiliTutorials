using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{

 /// <summary>
    /// Represents a quiz associated with a course.
    /// </summary>
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Instructions { get; set; }

        public int DurationInMinutes { get; set; }

        public DateTime AvailableFrom { get; set; }

        public DateTime AvailableUntil { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<QuizQuestion> Questions { get; set; }
        public ICollection<QuizAttempt> QuizAttempts { get; set; }
    }

}
