using System.Collections.Generic;
using api.Dtos.Student;

namespace api.Dtos.Course
{
    public class CourseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string Schedule { get; set; }

        public string Professor { get; set; }

        // Incluimos el listado de estudiantes inscritos
        public List<StudentDto> Students { get; set; }
    }
}
