using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
     /// <summary>
    /// Represents a tutor user.
    /// </summary>
    public class Tutor
    {
        [Key]
        public string UserId { get; set; }

        public User User { get; set; }

        [StringLength(200)]
        public string Qualifications { get; set; }

        public string SubjectsTaught { get; set; }

        // Navigation properties
        public ICollection<Course> Courses { get; set; }
        public ICollection<TutoringSession> TutoringSessions { get; set; }
        public ICollection<StudyMaterial> StudyMaterials { get; set; }
    }
}
