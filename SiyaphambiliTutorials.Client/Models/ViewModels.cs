using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SiyaphambiliTutorials.Data;

namespace SiyaphambiliTutorials.Client.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }


    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }


    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ManageAccountViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }

        // Role-specific data collections
        public List<CourseViewModel> Courses { get; set; } = new List<CourseViewModel>();
        public List<EnrollmentViewModel> Enrollments { get; set; } = new List<EnrollmentViewModel>();
        public List<AuditLogViewModel> AuditLogs { get; set; } = new List<AuditLogViewModel>();

        // Security settings
        public bool IsTwoFactorEnabled { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }


    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Level { get; set; }
        public string TutorName { get; set; }
        public decimal? Price { get; set; }
        public bool IsPublished { get; set; }
        public IFormFile ImageFile { get; set; }
        public List<ModuleViewModel> Modules { get; set; } = new List<ModuleViewModel>();
    }

    public class ModuleViewModel
    {
        public int CourseModuleId { get; set; }
        public string Title { get; set; }
        public List<ModuleContentViewModel> ModuleContents { get; set; } = new List<ModuleContentViewModel>();
    }


    public class EnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public string CourseTitle { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; }
        public double ProgressPercentage { get; set; }
    }

    public class AuditLogViewModel
    {
        public int AuditLogId { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }
    }

    public class ModuleContentViewModel
    {
        public int ModuleContentId { get; set; }
        public string ContentType { get; set; } // e.g., Video, Text, Quiz
        public string ContentUrl { get; set; }
        public int CourseModuleId { get; set; } // Foreign key reference to CourseModule
        public bool IsCompleted { get; set; } // For tracking if the content is completed
    }


    public class TutoringSessionViewModel
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Range(1, 50)]
        public int MaxParticipants { get; set; }

        public string MeetingLink { get; set; }
    }

    public class TutoringSessionDetailsViewModel
    {
        public TutoringSession Session { get; set; }
        public bool IsAlreadyBooked { get; set; }
        public int SpotsAvailable { get; set; }
    }



}

