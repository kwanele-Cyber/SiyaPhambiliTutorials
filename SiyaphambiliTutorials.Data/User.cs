using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SiyaphambiliTutorials.Data;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents a user in the system, extending IdentityUser for authentication.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// First name of the user.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Date when the user registered.
        /// </summary>
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates if the user account is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Indicates the role the user plays in the system.
        /// </summary>
        [Required]
        public UserRole Role { get; set; }

        // Navigation properties for roles

        public Student StudentProfile { get; set; }
        public Tutor TutorProfile { get; set; }
        public Administrator AdminProfile { get; set; }
    }
}
