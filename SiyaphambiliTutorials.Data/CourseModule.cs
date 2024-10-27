using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{ /// <summary>
    /// Represents a module within a course.
    /// </summary>
    public class CourseModule
    {
        [Key]
        public int CourseModuleId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<ModuleContent> ModuleContents { get; set; }
    }
}
