using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiyaphambiliTutorials.Data
{
    public enum AvailabilityStatus
    {
        Available,
        CheckedOut,
        Reserved,
        Lost
    }

    public enum LoanStatus
    {
        Active,
        Overdue,
        Completed
    }

    public enum ReservationStatus
    {
        Pending,
        Fulfilled,
        Cancelled
    }

    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse,
        ShortAnswer,
        Essay
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }

    public enum SessionStatus
    {
        Scheduled,
        InProgress,
        Completed,
        Cancelled
    }

    public enum UserRole
    {
        Student,
        Tutor,
        Administrator
    }

}
