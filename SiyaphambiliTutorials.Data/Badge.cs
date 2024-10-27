using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents a badge that users can earn.
    /// </summary>
    public class Badge
    {
        [Key]
        public int BadgeId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string IconPath { get; set; }

        public ICollection<UserBadge> UserBadges { get; set; }
    }
}
