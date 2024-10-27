using System;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
   /// <summary>
    /// Represents a post within a forum thread.
    /// </summary>
    public class ForumPost
    {
        [Key]
        public int ForumPostId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public string PostedByUserId { get; set; }
        public User PostedByUser { get; set; }

        public int ForumThreadId { get; set; }
        public ForumThread ForumThread { get; set; }
    }
}
