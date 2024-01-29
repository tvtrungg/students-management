using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Models.Entities;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students with scores
        public async Task<IActionResult> Index()
        {
            var studentsWithScores = await _context.Students
                .Where(s => s.Role != "Admin")
                .ToListAsync();

            var subjects = await _context.Subjects.ToListAsync();

            var studentScores = new List<StudentScore>();

            foreach (var student in studentsWithScores)
            {
                var studentRow = new StudentScore
                {
                    StudentId = student.Id,
                    StudentName = student.Name,
                    Scores = new List<ScoreInfo>()
                };

                foreach (var subject in subjects)
                {
                    var score = _context.Scores
                        .FirstOrDefault(s => s.StudentId == student.Id && s.SubjectId == subject.Id)?.Score;

                    var scoreInfo = new ScoreInfo
                    {
                        SubjectId = subject.Id,
                        Score = score
                    };

                    studentRow.Scores.Add(scoreInfo);
                }

                studentScores.Add(studentRow);
            }

            ViewBag.Subjects = subjects;
            ViewBag.StudentScores = studentScores;

            return View();
        }

        // GET: Home/EditScore/id
        public async Task<IActionResult> EditScore(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null || student.Role == "Admin")
            {
                return NotFound();
            }

            var subjects = await _context.Subjects.ToListAsync();
            var scores = await _context.Scores.Where(s => s.StudentId == id).ToListAsync();

            var studentScore = new StudentScore
            {
                StudentId = student.Id,
                StudentName = student.Name,
                Scores = new List<ScoreInfo>()
            };

            foreach (var subject in subjects)
            {
                var score = scores.FirstOrDefault(s => s.SubjectId == subject.Id)?.Score;

                var scoreInfo = new ScoreInfo
                {
                    SubjectId = subject.Id,
                    Score = score
                };

                studentScore.Scores.Add(scoreInfo);
            }

            ViewBag.Subjects = subjects;
            ViewBag.StudentScore = studentScore;  // Corrected ViewBag key

            return View();
        }

        // POST: Home/EditScore/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditScore(Guid id, StudentScore studentScore)
        {
            if (id != studentScore.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update scores in the database
                    foreach (var scoreInfo in studentScore.Scores)
                    {
                        var existingScore = await _context.Scores
                            .FirstOrDefaultAsync(s => s.StudentId == id && s.SubjectId == scoreInfo.SubjectId);

                        if (existingScore != null)
                        {
                            existingScore.Score = scoreInfo.Score;
                            _context.Update(existingScore);
                        }
                        else
                        {
                            var newScore = new Scores
                            {
                                StudentId = id,
                                SubjectId = scoreInfo.SubjectId,
                                Score = scoreInfo.Score ?? 0 // Default to 0 if score is null
                            };
                            _context.Add(newScore);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            var subjects = await _context.Subjects.ToListAsync();
            ViewBag.Subjects = subjects;
            ViewBag.StudentScore = studentScore;

            return View(studentScore);
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
