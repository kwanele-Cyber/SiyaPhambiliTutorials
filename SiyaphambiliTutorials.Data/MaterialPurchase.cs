using System;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents a purchase of a study material by a student.
    /// </summary>
    public class MaterialPurchase
    {
        [Key]
        public int MaterialPurchaseId { get; set; }

        public int StudyMaterialId { get; set; }
        public StudyMaterial StudyMaterial { get; set; }

        public string StudentId { get; set; }
        public Student Student { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        public decimal AmountPaid { get; set; }
    }
}
