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
    public class StudyMaterialsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public StudyMaterialsController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: List all study materials
        public async Task<IActionResult> Index()
        {
            var materials = await _context.StudyMaterials
                .Include(m => m.Tutor)
                .ToListAsync();
            return View(materials);
        }

        // GET: StudyMaterial Details
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var material = await _context.StudyMaterials
                .Select(m => new StudyMaterialViewModel
                {
                    StudyMaterialId = m.StudyMaterialId,
                    Title = m.Title,
                    Description = m.Description,
                    Price = m.Price,
                    MaterialUrl = m.MaterialUrl,
                    IsPurchased = _context.MaterialPurchases
                                    .Any(p => p.StudyMaterialId == m.StudyMaterialId && p.StudentId == userId)
                })
                .FirstOrDefaultAsync(m => m.StudyMaterialId == id);

            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }


        // GET: Create StudyMaterial (For Tutors)
        [Authorize(Roles = "Tutor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create StudyMaterial (For Tutors)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Create(StudyMaterial model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                var material = new StudyMaterial
                {
                    Title = model.Title,
                    Description = model.Description,
                    MaterialUrl = model.MaterialUrl,
                    Price = model.Price,
                    DateAdded = DateTime.UtcNow,
                    TutorId = userId
                };

                _context.StudyMaterials.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Edit StudyMaterial (For Tutors)
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var material = await _context.StudyMaterials
                .FirstOrDefaultAsync(m => m.StudyMaterialId == id && m.TutorId == userId);
            
            if (material == null) return NotFound();

            return View(material);
        }

        // POST: Edit StudyMaterial (For Tutors)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Edit(int id, StudyMaterial model)
        {
            if (id != model.StudyMaterialId) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(User);
                    var material = await _context.StudyMaterials
                        .FirstOrDefaultAsync(m => m.StudyMaterialId == id && m.TutorId == userId);
                    
                    if (material == null) return NotFound();

                    material.Title = model.Title;
                    material.Description = model.Description;
                    material.MaterialUrl = model.MaterialUrl;
                    material.Price = model.Price;

                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await StudyMaterialExists(model.StudyMaterialId))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Delete StudyMaterial (For Tutors)
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var material = await _context.StudyMaterials
                .FirstOrDefaultAsync(m => m.StudyMaterialId == id && m.TutorId == userId);

            if (material == null) return NotFound();

            return View(material);
        }

        // POST: Delete StudyMaterial (For Tutors)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var material = await _context.StudyMaterials
                .FirstOrDefaultAsync(m => m.StudyMaterialId == id && m.TutorId == userId);

            if (material == null) return NotFound();

            _context.StudyMaterials.Remove(material);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Purchase StudyMaterial
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(int materialId)
        {
            var userId = _userManager.GetUserId(User);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null) return Unauthorized();

            var material = await _context.StudyMaterials.FindAsync(materialId);
            if (material == null) return NotFound();

            // Check if student already purchased material
            var alreadyPurchased = await _context.MaterialPurchases
                .AnyAsync(mp => mp.StudyMaterialId == materialId && mp.StudentId == student.UserId);

            if (alreadyPurchased) return BadRequest("You have already purchased this material.");

            // Create purchase record
            var purchase = new MaterialPurchase
            {
                StudyMaterialId = materialId,
                StudentId = student.UserId,
                PurchaseDate = DateTime.UtcNow,
                AmountPaid = material.Price
            };

            _context.MaterialPurchases.Add(purchase);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyMaterials");
        }

        // GET: My Purchased Materials
        public async Task<IActionResult> MyMaterials()
        {
            var userId = _userManager.GetUserId(User);
            var purchasedMaterials = await _context.MaterialPurchases
                .Where(mp => mp.StudentId == userId)
                .Include(mp => mp.StudyMaterial)
                .ToListAsync();

            return View(purchasedMaterials);
        }

        private async Task<bool> StudyMaterialExists(int id)
        {
            return await _context.StudyMaterials.AnyAsync(e => e.StudyMaterialId == id);
        }
    }
}
