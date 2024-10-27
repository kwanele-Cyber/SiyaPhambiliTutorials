using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents an administrator user.
    /// </summary>
    public class Administrator
    {
        [Key]
        public string UserId { get; set; }

        public User User { get; set; }

        // Additional admin-specific properties can be added here
        public int AdminRoleId { get; set; }
        public virtual AdminRole AdminRole { get; set; }
    }
}
