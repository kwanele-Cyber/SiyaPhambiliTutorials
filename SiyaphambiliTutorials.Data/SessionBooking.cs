using System;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
     /// <summary>
    /// Represents a booking made by a student for a tutoring session.
    /// </summary>
    public class SessionBooking
    {
        [Key]
        public int SessionBookingId { get; set; }

        public int TutoringSessionId { get; set; }
        public TutoringSession TutoringSession { get; set; }

        public string StudentId { get; set; }
        public Student Student { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public bool IsConfirmed { get; set; } = true;
    }
}
