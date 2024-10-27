using System;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    public class UserBadge
    {
        public int UserBadgeId { get; set; }
    
        public int BadgeId { get; set; }
        public Badge Badge { get; set; }
    
        public string UserId { get; set; }
        public User User { get; set; }
    
        public DateTime EarnedDate { get; set; } = DateTime.UtcNow;
    }
}
