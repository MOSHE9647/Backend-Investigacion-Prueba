using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Student;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StudentController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET api/student
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _context.Students
                                         .Include(s => s.Course)
                                         .ToListAsync();
            var dto = students.Select(s => s.ToDto());
            return Ok(dto);
        }

        // GET api/student/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var student = await _context.Students
                                        .Include(s => s.Course)
                                        .FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
                return NotFound();
            return Ok(student.ToDto());
        }

        // POST api/student
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
        {
            // Verifica que el curso exista
            if (!await _context.Courses.AnyAsync(c => c.Id == dto.CourseId))
                return BadRequest($"No existe Course con Id = {dto.CourseId}");

            var student = dto.ToStudent();
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById), 
                new { id = student.Id }, 
                student.ToDto()
            );
        }

        // PUT api/student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateStudentDto dto
        )
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            // Verifica curso
            if (!await _context.Courses.AnyAsync(c => c.Id == dto.CourseId))
                return BadRequest($"No existe Course con Id = {dto.CourseId}");

            // Mapear campos
            student.Name     = dto.Name;
            student.Email    = dto.Email;
            student.Phone    = dto.Phone;
            student.CourseId = dto.CourseId;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            return Ok(student.ToDto());
        }

        // DELETE api/student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
