using System;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents a notification sent to a user.
    /// </summary>
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string Message { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public string NotificationType { get; set; } // e.g., System, Message, Reminder
    }
}
