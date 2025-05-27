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

        // Sólo para subir un fichero nuevo; opcional
        public IFormFile? File { get; set; }  
    }
}
