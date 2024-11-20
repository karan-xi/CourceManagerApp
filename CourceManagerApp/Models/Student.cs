using System.ComponentModel.DataAnnotations;

namespace CourceManagerApp.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Foreign key for Course
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // Enum for Enrollment Status
        public StatusEnrollment Status { get; set; } = StatusEnrollment.ConfirmationMessageNotSent;
    }


    
}
