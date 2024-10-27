using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiyaphambiliTutorials.Data
{
    public class StudentModuleContentCompletion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        public int ModuleContentId { get; set; }
        public ModuleContent ModuleContent { get; set; }

        public DateTime CompletionDate { get; set; } = DateTime.UtcNow;
    }

}