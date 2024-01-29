using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models.Entities;

namespace StudentManagement.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectsController(ApplicationDbContext context)
        {
            _context = context;

        }

        //[Authorize(Roles = "Admin")]
        // GET: Subjects
        public async Task<IActionResult> Index(string? searchString)
        {
            // Không render role Admin
            var subjects = from s in _context.Subjects
                           select s;

            if (!System.String.IsNullOrEmpty(searchString))
            {
                subjects = subjects.Where(s => s.Name.Contains(searchString));
            }

            return View(await subjects.ToListAsync());
        }


        // GET: Subjects/CreateSubject
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateSubject()
        {
            return View();
        }

        // POST: Subjects/CreateSubject
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubject(Subject subject)
        {
            var subjectInDb = _context.Subjects.FirstOrDefault(s => s.Id == subject.Id);
            if (subjectInDb == null)
            {
                subject.Id = Guid.NewGuid();
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subjectInDb);
        }

        // GET: Subjects/EditSubject
        [HttpGet]
        public async Task<IActionResult> EditSubject(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Subjects/EditSubject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubject(Subject subject)
        {
            var subjectInDb = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == subject.Id);
            if (subjectInDb != null)
            {
                subjectInDb.Code = subject.Code;
                subjectInDb.Name = subject.Name;
                subjectInDb.Description = subject.Description;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subjectInDb);
        }

        // GET: Subjects/DeleteSubject

        [HttpGet]
        public async Task<IActionResult> DeleteSubject(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        [HttpPost, ActionName("DeleteSubject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var subject = await _context.Subjects.FindAsync(id);

            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
