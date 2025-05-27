using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Controllers;
using api.Dtos.Course;

namespace tests;

public class DeleteCourseTest
{
    private readonly ApplicationDBContext context;
    private readonly CourseController controller;

    public DeleteCourseTest()
    {
        context = TestHelpers.GetDbContext();
        controller = TestHelpers.GetController(context);
    }

    [Fact]
    public async Task DeleteCourse_HappyPath_ReturnsNoContentAndRemovesCourse()
    {
        // Arrange: Creates a valid course first to update
        // (This is necessary because the update method requires an existing course)
        var createDto = new CreateCourseDto
        {
            Name = "Math",
            Description = "Desc",
            File = TestHelpers.GetMockFormFile(),
            Schedule = "Mon",
            Professor = "Prof"
        };
        var createResult = await controller.Create(createDto);
        var created = Assert.IsType<CreatedAtActionResult>(createResult);
        var course = Assert.IsType<CourseDto>(created.Value);

        // Act: Call Delete with the course's id
        var result = await controller.Delete(course.Id);

        // Assert: Should return NoContent and course should be removed
        Assert.IsType<NoContentResult>(result);
        var deleted = await context.Courses.FindAsync(course.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteCourse_NonExistentId_ReturnsNotFound()
    {
        // Arrange: Use an id that does not exist
        int nonExistentId = 9999;

        // Act: Call Delete with non-existent id
        var result = await controller.Delete(nonExistentId);

        // Assert: Should return NotFound
        Assert.IsType<NotFoundResult>(result);
    }
}