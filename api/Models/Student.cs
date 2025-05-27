namespace api.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        // Foreign key al curso
        public int CourseId { get; set; }

        // Navegación hacia Course
        public Course Course { get; set; }
    }
}
