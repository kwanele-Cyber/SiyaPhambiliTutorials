using System;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents an internal message between users.
    /// </summary>
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string SenderId { get; set; }
        public User Sender { get; set; }

        public string RecipientId { get; set; }
        public User Recipient { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime SentDate { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
    
}
