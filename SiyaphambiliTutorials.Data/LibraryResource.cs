using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
    /// <summary>
    /// Represents a resource in the library.
    /// </summary>
    public class LibraryResource
    {
        [Key]
        public int LibraryResourceId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }

        public string ResourceType { get; set; } // e.g., Book, Journal, DVD

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public ICollection<ResourceLoan> ResourceLoans { get; set; }
    }
}
