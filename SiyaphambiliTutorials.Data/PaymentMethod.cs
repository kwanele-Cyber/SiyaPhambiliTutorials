using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
     /// <summary>
    /// Represents a payment method associated with a user.
    /// </summary>
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string PaymentType { get; set; } // e.g., CreditCard, PayPal

        public string Provider { get; set; } // e.g., Visa, MasterCard

        public string AccountNumberMasked { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
