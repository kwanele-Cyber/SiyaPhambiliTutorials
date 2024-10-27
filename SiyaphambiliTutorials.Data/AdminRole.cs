using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    public class AdminRole
    {
        public int AdminRoleId { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Administrator> Administrators { get; set; }
    }

}