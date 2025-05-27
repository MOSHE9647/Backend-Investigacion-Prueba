using System.Collections.Generic;

namespace api.Models
{
    public class Course
    {
        public Course()
        {
            Students = new List<Student>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string Schedule { get; set; }

        public string Professor { get; set; }

        // Relación 1-N
        public ICollection<Student> Students { get; set; }
    }
}
