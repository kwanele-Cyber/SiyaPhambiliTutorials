using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiyaphambiliTutorials.Data
{
/// <summary>
    /// Represents a tutoring session scheduled by a tutor.
    /// </summary>
    public class TutoringSession
    {
        [Key]
        public int TutoringSessionId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int MaxParticipants { get; set; }

        [Required]
        public string TutorId { get; set; }
        public Tutor Tutor { get; set; }

        public ICollection<SessionBooking> SessionBookings { get; set; }

        public string MeetingLink { get; set; } 

        public string MeetingRoomName { get; set; } = Guid.NewGuid().ToString();

        public bool IsCancelled { get; set; } = false;
    }

}
