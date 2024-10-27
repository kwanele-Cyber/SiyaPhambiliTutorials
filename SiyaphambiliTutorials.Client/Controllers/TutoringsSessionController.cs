using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiyaphambiliTutorials.Data;

namespace SiyaphambiliTutorials.Controllers
{
    [Authorize]
    public class TutoringSessionsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public TutoringSessionsController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: List all available tutoring sessions
        public async Task<IActionResult> Index()
        {
            var sessions = await _context.TutoringSessions
                .Include(s => s.Tutor)
                .Where(s => s.StartTime >= DateTime.UtcNow && !s.IsCancelled)
                .ToListAsync();
            return View(sessions);
        }

        // GET: Show form to create a tutoring session (tutors only)
        [Authorize(Roles = "Tutor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create a new tutoring session
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Create(TutoringSession model)
        {
            if (ModelState.IsValid)
            {
                var meetingRoomName = model.MeetingRoomName+"_"+Guid.NewGuid().ToString();

                var session = new TutoringSession
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartTime = model.StartTime.ToUniversalTime(),
                    EndTime = model.EndTime.ToUniversalTime(),
                    MaxParticipants = model.MaxParticipants,
                    TutorId = _userManager.GetUserId(User),
                    MeetingRoomName = meetingRoomName,
                    MeetingLink = $"https://meet.jit.si/{meetingRoomName}"  // Jitsi Meeting Link
                };

                _context.TutoringSessions.Add(session);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Book a session for a student
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Book(int id)
        {
            var session = await _context.TutoringSessions
                .Include(s => s.SessionBookings)
                .FirstOrDefaultAsync(s => s.TutoringSessionId == id);

            if (session == null || session.IsCancelled || session.StartTime <= DateTime.UtcNow)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            // Prevent double booking
            if (session.SessionBookings.Any(sb => sb.StudentId == userId))
            {
                return BadRequest("You have already booked this session.");
            }

            // Check if the session is at max capacity
            if (session.SessionBookings.Count >= session.MaxParticipants)
            {
                return BadRequest("This session is fully booked.");
            }

            var booking = new SessionBooking
            {
                TutoringSessionId = id,
                StudentId = userId,
                BookingDate = DateTime.UtcNow,
                IsConfirmed = true
            };

            _context.SessionBookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("MySessions");
        }

        // GET: Manage a tutor's sessions
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> ManageSessions()
        {
            var tutorId = _userManager.GetUserId(User);
            var sessions = await _context.TutoringSessions
                .Where(s => s.TutorId == tutorId && s.StartTime >= DateTime.UtcNow)
                .Include(s => s.SessionBookings)
                .ToListAsync();

            return View(sessions);
        }

        // POST: Cancel a session (for tutors)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Cancel(int id)
        {
            var tutorId = _userManager.GetUserId(User);
            var session = await _context.TutoringSessions
                .FirstOrDefaultAsync(s => s.TutoringSessionId == id && s.TutorId == tutorId);

            if (session == null)
            {
                return NotFound();
            }

            session.IsCancelled = true;
            _context.TutoringSessions.Update(session);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageSessions");
        }

        // GET: List student's booked sessions
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MySessions()
        {
            var studentId = _userManager.GetUserId(User);
            var bookings = await _context.SessionBookings
                .Include(b => b.TutoringSession)
                .Where(b => b.StudentId == studentId && b.TutoringSession.StartTime >= DateTime.UtcNow && !b.TutoringSession.IsCancelled)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Display session details and option to book
        public async Task<IActionResult> Details(int id)
        {
            var session = await _context.TutoringSessions
                .Include(s => s.SessionBookings)
                .FirstOrDefaultAsync(s => s.TutoringSessionId == id);

            if (session == null)
                return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            var isAlreadyBooked = session.SessionBookings.Any(b => b.StudentId == currentUserId);

            var model = new TutoringSessionDetailsViewModel
            {
                Session = session,
                IsAlreadyBooked = isAlreadyBooked,
                SpotsAvailable = session.MaxParticipants - session.SessionBookings.Count
            };

            return View(model);
        }

        // POST: Book a session
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookSession(int sessionId)
        {
            var session = await _context.TutoringSessions
                .Include(s => s.SessionBookings)
                .FirstOrDefaultAsync(s => s.TutoringSessionId == sessionId);

            if (session == null || session.SessionBookings.Count >= session.MaxParticipants)
                return BadRequest("Session is fully booked.");

            var userId = _userManager.GetUserId(User);
            var isAlreadyBooked = session.SessionBookings.Any(b => b.StudentId == userId);

            if (!isAlreadyBooked)
            {
                var booking = new SessionBooking
                {
                    TutoringSessionId = sessionId,
                    StudentId = userId,
                    BookingDate = DateTime.UtcNow,
                    IsConfirmed = true
                };

                _context.SessionBookings.Add(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = sessionId });
        }
    }


}
