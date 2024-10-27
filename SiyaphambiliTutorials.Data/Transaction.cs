using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiyaphambiliTutorials.Data
{
     /// <summary>
    /// Represents a financial transaction made by a user.
    /// </summary>
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public string TransactionType { get; set; } // e.g., Purchase, Rental

        public string Status { get; set; } // e.g., Success, Failed

        public string ReferenceNumber { get; set; }
    }
}
