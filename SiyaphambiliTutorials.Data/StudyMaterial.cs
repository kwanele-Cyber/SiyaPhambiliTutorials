using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents a study material that can be purchased or accessed.
    /// </summary>
    public class StudyMaterial
    {
        [Key]
        public int StudyMaterialId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string MaterialUrl { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        [Required]
        public string TutorId { get; set; }
        public Tutor Tutor { get; set; }

        public ICollection<MaterialPurchase> MaterialPurchases { get; set; }
    }

}
