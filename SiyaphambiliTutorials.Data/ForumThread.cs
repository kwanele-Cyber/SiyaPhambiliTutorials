using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents a thread within a forum topic.
    /// </summary>
    public class ForumThread
    {
        [Key]
        public int ForumThreadId { get; set; }

        [Required]
        public string Subject { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }

        public int ForumTopicId { get; set; }
        public ForumTopic ForumTopic { get; set; }

        public ICollection<ForumPost> ForumPosts { get; set; }
    }
}
