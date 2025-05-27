using api.Dtos.Student;
using api.Models;

namespace api.Mappers
{
    public static class StudentMapper
    {
        // De CreateStudentDto → Student
        public static Student ToStudent(this CreateStudentDto dto) => new()
        {
            Name     = dto.Name,
            Email    = dto.Email,
            Phone    = dto.Phone,
            CourseId = dto.CourseId
        };

        // Mapear para Update: opcional, puedes usar directamente ToStudent o hacerlo inline
        public static void MapFrom(this Student student, UpdateStudentDto dto)
        {
            student.Name     = dto.Name;
            student.Email    = dto.Email;
            student.Phone    = dto.Phone;
            student.CourseId = dto.CourseId;
        }

        // De Student → StudentDto
        public static StudentDto ToDto(this Student s) => new()
        {
            Id       = s.Id,
            Name     = s.Name,
            Email    = s.Email,
            Phone    = s.Phone,
            CourseId = s.CourseId
        };
    }
}
