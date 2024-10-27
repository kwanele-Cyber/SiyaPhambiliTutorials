using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
     /// <summary>
    /// Represents a student user.
    /// </summary>
    public class Student
    {
        [Key]
        public string UserId { get; set; }

        public User User { get; set; }

        [StringLength(100)]
        public string AcademicLevel { get; set; }

        [StringLength(100)]
        public string FieldOfStudy { get; set; }

        // Navigation properties
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<QuizAttempt> QuizAttempts { get; set; }
        public ICollection<ResourceLoan> ResourceLoans { get; set; }
        public ICollection<SessionBooking> SessionBookings { get; set; }
        public ICollection<MaterialPurchase> MaterialPurchases { get; set; }
    }
}
