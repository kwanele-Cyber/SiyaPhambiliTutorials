using System;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents a loan of a library resource to a student.
    /// </summary>
    public class ResourceLoan
    {
        [Key]
        public int ResourceLoanId { get; set; }

        public int LibraryResourceId { get; set; }
        public LibraryResource LibraryResource { get; set; }

        public string StudentId { get; set; }
        public Student Student { get; set; }

        public DateTime LoanDate { get; set; } = DateTime.UtcNow;

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; } = false;

        public decimal FineAmount { get; set; }
    }

}
