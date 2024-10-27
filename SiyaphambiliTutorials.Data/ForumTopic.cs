using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    
    /// <summary>
    /// Represents a topic in the discussion forum.
    /// </summary>
    public class ForumTopic
    {
        [Key]
        public int ForumTopicId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }

        public ICollection<ForumThread> ForumThreads { get; set; }
    }
}
