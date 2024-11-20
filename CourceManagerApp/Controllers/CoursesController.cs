using CourceManagerApp.Models;
using CourceManagerApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CourceManagerApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        

        public CoursesController(ApplicationDbContext context, EmailService EmailService )
        {
            _context = context;
            _emailService = EmailService;
            
        }

        public IActionResult Index()
        {
            var courses = _context.Course
             .Include(c => c.Students)
             .ToList();  // Load all courses

            return View(courses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Course.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public IActionResult Edit(int id)
        {
            var course = _context.Course.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course updatedCourse)
        {
            if (ModelState.IsValid)
            {
                var course = _context.Course.FirstOrDefault(c => c.Id == id);
                if (course == null)
                {
                    return NotFound();
                }

                course.Name = updatedCourse.Name;
                course.Instructor = updatedCourse.Instructor;
                course.StartDate = updatedCourse.StartDate;
                course.RoomNumber = updatedCourse.RoomNumber;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(updatedCourse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(int CourseId, string Name, string Email)
        {
            var course = await _context.Course
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == CourseId);

            if (course == null)
            {
                return NotFound();
            }

            // Create the new student
            var student = new Student
            {
                Name = Name,
                Email = Email,
                Status = StatusEnrollment.ConfirmationMessageNotSent, // Set default status
                CourseId = CourseId
            };

            // Add the student to the course
            course.Students.Add(student);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect back to the course management page
            return RedirectToAction(nameof(Manage), new { id = CourseId });
        }

        // Manage action (already present in your controller)
        public IActionResult Manage(int id)
        {
            var course = _context.Course
                .Include(c => c.Students)
                .FirstOrDefault(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendConfirmation(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }

            try
            {
                // Send the confirmation email
                await _emailService.SendConfirmationEmail(student.Email, student.Name, student.CourseId);

                // Update the student's status
                student.Status = StatusEnrollment.ConfirmationMessageSent;
                _context.Update(student);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Confirmation email sent successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to send confirmation email.";

            }

            return RedirectToAction(nameof(Manage), new { id = student.CourseId });
        }
    }
}