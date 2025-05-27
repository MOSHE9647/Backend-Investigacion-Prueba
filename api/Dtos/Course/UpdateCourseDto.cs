using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Course
{
    public class UpdateCourseDto
    {
        [Required] public string Name        { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string Schedule    { get; set; }
        [Required] public string Professor   { get; set; }

        // Only used if we want to update the image
        public IFormFile? File { get; set; }  
    }
}
