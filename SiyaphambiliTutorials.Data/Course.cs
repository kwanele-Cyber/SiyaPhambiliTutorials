using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiyaphambiliTutorials.Data
{
/// <summary>
    /// Represents a course offered on the platform.
    /// </summary>
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course title is required.")]
        [StringLength(200, ErrorMessage = "Course title cannot exceed 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Course description is required.")]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters.")]
        public string Category { get; set; }

        public string ImageUrl { get; set; }

        [StringLength(50)]
        public string Level { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public bool IsPublished { get; set; } = false;

        [Required]
        public string TutorId { get; set; }

        [ForeignKey("TutorId")]
        public Tutor Tutor { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<CourseModule> Modules { get; set; }
        public ICollection<Quiz> Quizzes { get; set; }

        [Range(0, 5)]
        public double? AverageRating { get; set; }

        public int RatingCount { get; set; } = 0;

        public bool IsApproved { get; set; } = false;

        public string Notes { get; set; }
    }
}
