using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiyaphambiliTutorials.Data;
using System.Threading.Tasks;
using SiyaphambiliTutorials.Client.Models;
using SiyaphambiliTutorials.Client.Controllers;
using Microsoft.EntityFrameworkCore;

namespace SiyaphambiliTutorials.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Prevent registration as Administrator
                if (model.Role == UserRole.Administrator)
                {
                    ModelState.AddModelError(string.Empty, "Registering as Administrator is not allowed.");
                    return View(model);
                }

                var user = new User { FirstName = model.FirstName, LastName = model.LastName, UserName = model.Email, Email = model.Email, Role = model.Role };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Here, you would typically assign the role to the user
                    await _userManager.AddToRoleAsync(user, model.Role.ToString());

                    // Log the user in
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                AddErrors(result);
            }
            return View(model);
        }


        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // GET: Account/ResetPassword
        public IActionResult ResetPassword()
        {
            return View();
        }

        // POST: Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        // GET: Account/ResetPasswordConfirmation
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // GET: Account/ChangePassword
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            AddErrors(result);
            return View(model);
        }


        public async Task<IActionResult> ManageAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ManageAccountViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Courses = new List<CourseViewModel>(),
                Enrollments = new List<EnrollmentViewModel>(),
                AuditLogs = new List<AuditLogViewModel>()
            };

            // Populate model based on the user's role
            if (user.Role == UserRole.Tutor)
            {
                model.Courses = await _context.Courses
                    .Where(c => c.Tutor.UserId == user.Id)
                    .Select(c => new CourseViewModel { Title = c.Title, Description = c.Description })
                    .ToListAsync();
            }
            else if (user.Role == UserRole.Student)
            {
                model.Enrollments = await _context.Enrollments
                    .Where(e => e.Student.UserId == user.Id)
                    .Select(e => new EnrollmentViewModel { CourseTitle = e.Course.Title, IsActive = e.IsActive })
                    .ToListAsync();
            }
            else if (user.Role == UserRole.Administrator)
            {
                model.AuditLogs = await _context.AuditLogs
                    .Select(a => new AuditLogViewModel { ActionDate = a.ActionDate, Description = a.Description })
                    .ToListAsync();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ManageAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageAccount", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login"); // or handle as unauthorized/error
            }

            // Updating the user's properties
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Optionally, you might want to re-sign in the user to refresh the identity cookie
                await _signInManager.RefreshSignInAsync(user);
                ViewBag.Message = "Your profile has been updated successfully.";
                return RedirectToAction("ManageAccount"); // Redirect back to the profile page or display success message
            }

            // If we got this far, something failed, redisplay form
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("ManageAccount", model);
        }




        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
