using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiyaphambiliTutorials.Client.Models;
using SiyaphambiliTutorials.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SiyaphambiliTutorials.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public CoursesController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: List all courses
        public async Task<IActionResult> Index(string? searchTerm = null, string? category = null, string? level = null, string? sortBy = null)
        {
            var coursesQuery = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                coursesQuery = coursesQuery.Where(c => c.Title.Contains(searchTerm) || c.Description.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(category))
            {
                coursesQuery = coursesQuery.Where(c => c.Category == category);
            }

            if (!string.IsNullOrEmpty(level))
            {
                coursesQuery = coursesQuery.Where(c => c.Level == level);
            }

            switch (sortBy)
            {
                case "date":
                    coursesQuery = coursesQuery.OrderByDescending(c => c.DateCreated);
                    break;
                case "popularity":
                    // Implement logic based on your criteria for popularity
                    break;
                case "cost":
                    coursesQuery = coursesQuery.OrderBy(c => c.Price);
                    break;
                default:
                    coursesQuery = coursesQuery.OrderBy(c => c.Title);
                    break;
            }

            var courses = await coursesQuery.Select(c => new CourseViewModel
            {
                CourseId = c.CourseId,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                IsPublished = c.IsPublished,
                TutorName = c.Tutor.User.FirstName + " " + c.Tutor.User.LastName
            }).ToListAsync();

            return View(courses);
        }



        // GET: Show create course form
        [Authorize(Roles = "Tutor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create a new course
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var imageBase64Data = model.ImageFile != null ? ConvertToBase64(model.ImageFile) : null;
                var course = new Course
                {
                    Title = model.Title,
                    Description = model.Description,
                    Category = model.Category,
                    Level = model.Level,
                    Price = model.Price,
                    DateCreated = DateTime.UtcNow,
                    TutorId = _userManager.GetUserId(User),
                    ImageUrl = imageBase64Data,
                    IsPublished = model.IsPublished
                };

                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private string ConvertToBase64(IFormFile file)
        {
            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    return Convert.ToBase64String(fileBytes);
                }
            }
            return null;
        }


        // GET: Enroll in a course
        // GET: Display enrollment confirmation page
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Enroll(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            // Optional: Check if the user is already enrolled
            var userId = _userManager.GetUserId(User);
            bool isEnrolled = await _context.Enrollments.AnyAsync(e => e.CourseId == id && e.StudentId == userId);
            if (isEnrolled)
            {
                return RedirectToAction("Index", new { message = "You are already enrolled in this course." });
            }

            return View(course);
        }

        // POST: Process the enrollment
        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollConfirm(int courseId)
        {
            var userId = _userManager.GetUserId(User);
            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = userId,
                EnrollmentDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Courses", new { id = courseId });
        }

        // GET: Manage content in a course
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> ManageContent(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Modules)
                .ThenInclude(m => m.ModuleContents)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null)
            {
                return NotFound();
            }

            if (course.TutorId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }

            // Map to CourseViewModel
            var model = new CourseViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                Category = course.Category,
                Level = course.Level,
                Price = course.Price,
                IsPublished = course.IsPublished,
                Modules = course.Modules.Select(m => new ModuleViewModel
                {
                    CourseModuleId = m.CourseModuleId,
                    Title = m.Title,
                    ModuleContents = m.ModuleContents.Select(mc => new ModuleContentViewModel
                    {
                        ModuleContentId = mc.ModuleContentId,
                        ContentType = mc.ContentType,
                        ContentUrl = mc.ContentUrl,
                        IsCompleted = false // Placeholder; update logic if necessary
                    }).ToList()
                }).ToList()
            };

            return View(model);
        }


        // POST: Add content to a course module
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> AddContent(ModuleContentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var moduleContent = new ModuleContent
                {
                    ContentType = model.ContentType,
                    ContentUrl = model.ContentUrl,
                    CourseModuleId = model.CourseModuleId
                };

                _context.ModuleContents.Add(moduleContent);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageContent", new { courseId = model.CourseModuleId });
            }
            return View(model);
        }

        // POST: Update student progress
        // Method to calculate course progress based on completed contents
        private async Task<double> CalculateCourseProgress(int courseId, string studentId)
        {
            // Get all modules and contents for the course
            var modules = await _context.CourseModules
                .Include(m => m.ModuleContents)
                .Where(m => m.CourseId == courseId)
                .ToListAsync();

            double totalWeight = modules.Sum(m => m.ModuleContents.Count);
            double completedWeight = 0;

            foreach (var module in modules)
            {
                // Count completed contents for this student in the module
                int completedContents = await _context.StudentModuleContentCompletions
                    .CountAsync(c => c.ModuleContent.CourseModuleId == module.CourseModuleId && c.StudentId == studentId);

                double moduleWeight = (double)module.ModuleContents.Count / totalWeight;
                completedWeight += moduleWeight * completedContents;
            }

            // Calculate percentage progress
            return (completedWeight / totalWeight) * 100;
        }


        // POST: Update progress after content completion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteContent(int contentId)
        {
            var userId = _userManager.GetUserId(User);
            var content = await _context.ModuleContents
                .Include(c => c.CourseModule)
                .ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(c => c.ModuleContentId == contentId);

            if (content == null)
            {
                return NotFound();
            }

            // Check if the student has already completed this content
            bool isAlreadyCompleted = await _context.StudentModuleContentCompletions
                .AnyAsync(c => c.ModuleContentId == contentId && c.StudentId == userId);

            if (!isAlreadyCompleted)
            {
                var completion = new StudentModuleContentCompletion
                {
                    StudentId = userId,
                    ModuleContentId = contentId
                };

                _context.StudentModuleContentCompletions.Add(completion);
                await _context.SaveChangesAsync();

                // Recalculate progress for the student's enrollment
                var enrollment = await _context.Enrollments
                    .FirstOrDefaultAsync(e => e.CourseId == content.CourseModule.CourseId && e.StudentId == userId);

                if (enrollment != null)
                {
                    enrollment.ProgressPercentage = await CalculateCourseProgress(content.CourseModule.CourseId, userId);
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("MyCourses");
        }

    }
}
