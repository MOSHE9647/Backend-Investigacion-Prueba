using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Student
{
    public class CreateStudentDto
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}
