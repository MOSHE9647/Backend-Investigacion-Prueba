using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Course;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _env;

        public CourseController(ApplicationDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env     = env;
        }

        // GET api/course
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _context.Courses
                                        .Include(c => c.Students)
                                        .ToListAsync();
            return Ok(courses.Select(c => c.ToDto()));
        }

        // GET api/course/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var course = await _context.Courses
                                       .Include(c => c.Students)
                                       .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();
            return Ok(course.ToDto());
        }

        // POST api/course
        [HttpPost]
public async Task<IActionResult> Create([FromForm] CreateCourseDto dto)
{
    if (dto.File == null || dto.File.Length == 0)
        return BadRequest("Debe subir un fichero de imagen.");

    var course = dto.ToCourse();
    course.ImageUrl = "";
    await _context.Courses.AddAsync(course);
    await _context.SaveChangesAsync();

    // Guardar imagen
    var uploads = Path.Combine(_env.WebRootPath, "UploadedImages");
    Directory.CreateDirectory(uploads);
    var ext = Path.GetExtension(dto.File.FileName);
    var fileName = $"{course.Id}{ext}";
    var path = Path.Combine(uploads, fileName);
    using var fs = System.IO.File.Create(path);
    await dto.File.CopyToAsync(fs);

    course.ImageUrl = $"/UploadedImages/{fileName}";
    _context.Courses.Update(course);
    await _context.SaveChangesAsync();


    return CreatedAtAction(nameof(GetById), new { id = course.Id }, course.ToDto());
}


        // PUT api/course/5
        [HttpPut("{id}")]
public async Task<IActionResult> Update(
    [FromRoute] int id,
    [FromForm]   UpdateCourseDto dto
)
{
    var course = await _context.Courses.FindAsync(id);
    if (course == null) return NotFound();

    // Si nos llega un fichero, guardarlo y actualizar la URL
    if (dto.File != null && dto.File.Length > 0)
    {
        course.ImageUrl = await SaveUploadedFile(dto.File, course.Id);
    }
    // Si dto.File == null, dejamos course.ImageUrl intacto

    // Mapeamos el resto de campos
    course.Name        = dto.Name;
    course.Description = dto.Description;
    course.Schedule    = dto.Schedule;
    course.Professor   = dto.Professor;

    await _context.SaveChangesAsync();
    return Ok(course.ToDto());
}


private async Task<string> SaveUploadedFile(IFormFile file, int id)
{
    var uploads = Path.Combine(_env.WebRootPath, "UploadedImages");
    Directory.CreateDirectory(uploads);

    var ext      = Path.GetExtension(file.FileName);
    var fileName = $"{id}{ext}";
    var path     = Path.Combine(uploads, fileName);

    using var stream = System.IO.File.Create(path);
    await file.CopyToAsync(stream);

    return $"/UploadedImages/{fileName}";
}

        // DELETE api/course/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
