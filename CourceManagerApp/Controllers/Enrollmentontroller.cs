using CourceManagerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment02_CourseManagement.Controllers
{
    // Controller for managing enrollments in the application
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  Action to confirm student enrollment based on course ID and student email
        public async Task<IActionResult> Confirm(int courseId, string studentEmail)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.CourseId == courseId && s.Email == studentEmail);

            if (student == null)
            {
                return NotFound();
            }

            // Update the student's enrollment status to "EnrollmentConfirmed"
            student.Status = StatusEnrollment.EnrollmentConfirmed;
            _context.Update(student);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Thank you for confirming your enrollment!";
            return View();
        }
    }
}
