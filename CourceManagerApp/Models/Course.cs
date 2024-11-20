using System.ComponentModel.DataAnnotations;

namespace CourceManagerApp.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Instructor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [RegularExpression(@"^\d[A-Z]\d{2}$", ErrorMessage = "Room number must be in the format: a digit, a capital letter, and two digits (e.g., 3G15).")]
        public string RoomNumber { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
      
    }
}
