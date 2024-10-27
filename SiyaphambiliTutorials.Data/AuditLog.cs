using System;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents an audit log entry for administrative actions.
    /// </summary>
    public class AuditLog
    {
        [Key]
        public int AuditLogId { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        public string ActionType { get; set; } // e.g., CreateUser, DeleteCourse

        public string Description { get; set; }
    }
}
