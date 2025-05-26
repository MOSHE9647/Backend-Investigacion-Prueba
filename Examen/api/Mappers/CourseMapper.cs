using api.Dtos.Course;
using api.Dtos.Student;
using api.Models;
using System.Linq;

namespace api.Mappers
{
    public static class CourseMapper
    {
        // CreateCourseDto ya no tiene ImageUrl, lo pondremos tras guardar el archivo
        public static Course ToCourse(this CreateCourseDto dto) => new()
        {
            Name        = dto.Name,
            Description = dto.Description,
            Schedule    = dto.Schedule,
            Professor   = dto.Professor
        };

        public static CourseDto ToDto(this Course course) => new()
        {
            Id          = course.Id,
            Name        = course.Name,
            Description = course.Description,
            ImageUrl    = course.ImageUrl,  // aquí sí leemos lo que guardó el controlador
            Schedule    = course.Schedule,
            Professor   = course.Professor,
            Students    = course.Students?
                             .Select(s => s.ToDto())
                             .ToList() 
                         ?? new List<StudentDto>()
        };
    }
}
