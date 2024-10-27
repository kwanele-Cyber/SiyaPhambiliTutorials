using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiyaphambiliTutorials.Data
{
     /// <summary>
    /// Represents the enrollment of a student in a course.
    /// </summary>
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        public string StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Additional properties for progress tracking
        public double ProgressPercentage { get; set; } = 0.0;

        public DateTime? CompletionDate { get; set; }

        
    }
}
