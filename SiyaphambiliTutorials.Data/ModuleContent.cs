using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{/// <summary>
    /// Represents content within a course module.
    /// </summary>
    public class ModuleContent
    {
        [Key]
        public int ModuleContentId { get; set; }

        [Required]
        public string ContentType { get; set; } // e.g., Video, Text, Quiz

        [Required]
        public string ContentUrl { get; set; } // URL or path to the content

        public int CourseModuleId { get; set; }
        public CourseModule CourseModule { get; set; }

        public bool Completed { get; set; } = false;  // New field to track if content is completed
    }
}
