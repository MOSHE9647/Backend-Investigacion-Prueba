using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Course
{
    public class CreateCourseDto
    {
      [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        public IFormFile? File { get; set; }
        [Required] public string Schedule { get; set; }
        [Required] public string Professor { get; set; }
    }
}
