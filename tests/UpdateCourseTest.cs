using Microsoft.AspNetCore.Mvc;
using api.Dtos.Course;
using api.Controllers;
using api.Data;

namespace tests;

public class UpdateCourseTest
{

    private readonly ApplicationDBContext context;
    private readonly CourseController controller;

    public UpdateCourseTest()
    {
        context = TestHelpers.GetDbContext();
        controller = TestHelpers.GetController(context);
    }

    [Fact]
    public async Task UpdateCourse_HappyPath_ReturnsUpdatedCourse()
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

        // Now we prepare the update DTO
        var updateDto = new UpdateCourseDto
        {
            Name = "Mathematics Updated",
            Description = "Updated desc",
            Schedule = "Tue",
            Professor = "Prof Updated",
            File = TestHelpers.GetMockFormFile()
        };

        // Act: Call Update with the course's id and the update DTO
        var result = await controller.Update(course.Id, updateDto);

        // Assert: Should return Ok and the course should be updated
        var okResult = Assert.IsType<OkObjectResult>(result);
        var updatedCourse = Assert.IsType<CourseDto>(okResult.Value);
        Assert.Equal(updateDto.Name, updatedCourse.Name);
        Assert.Equal(updateDto.Description, updatedCourse.Description);
        Assert.Equal(updateDto.Schedule, updatedCourse.Schedule);
        Assert.Equal(updateDto.Professor, updatedCourse.Professor);
        Assert.NotNull(updatedCourse.ImageUrl);
    }

    [Fact]
    public async Task UpdateCourse_InvalidId_ReturnsNotFound()
    {
        // Arrange: Creates a valid update DTO
        var updateDto = new UpdateCourseDto
        {
            Name = "Any",
            Description = "Any",
            Schedule = "Any",
            Professor = "Any",
            File = TestHelpers.GetMockFormFile()
        };

        // Act: Use an unlikely ID to simulate a non-existent course
        var id = 999; // Unlikely ID
        var result = await controller.Update(id, updateDto);

        // Assert: Should return NotFound
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateCourse_InvalidName_ReturnsBadRequest()
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

        // Invalid update DTO
        var updateDto = new UpdateCourseDto
        {
            Name = "", // Invalid name
            Description = "Desc",
            Schedule = "Mon",
            Professor = "Prof",
            File = TestHelpers.GetMockFormFile()
        };

        // Act: Call Update with the course's id and the update DTO
        var result = await controller.Update(course.Id, updateDto);

        // Assert: Should return BadRequest
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCourse_InvalidDescription_ReturnsBadRequest()
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

        // Invalid update DTO
        var updateDto = new UpdateCourseDto
        {
            Name = "Math Updated",
            Description = "", // Invalid description
            Schedule = "Mon",
            Professor = "Prof",
            File = TestHelpers.GetMockFormFile()
        };

        // Act: Call Update with the course's id and the update DTO
        var result = await controller.Update(course.Id, updateDto);

        // Assert: Should return BadRequest
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCourse_NoFile_ReturnsUpdatedCourseWithoutImage()
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

        // Now we prepare the update DTO without a file
        var updateDto = new UpdateCourseDto
        {
            Name = "Mathematics Updated",
            Description = "Updated desc",
            Schedule = "Tue",
            Professor = "Prof Updated",
            File = null // No file provided
        };

        // Act: Call Update with the course's id and the update DTO
        var result = await controller.Update(course.Id, updateDto);

        // Assert: Should return Ok and the course should be updated
        var okResult = Assert.IsType<OkObjectResult>(result);
        var updatedCourse = Assert.IsType<CourseDto>(okResult.Value);
        Assert.Equal(updateDto.Name, updatedCourse.Name);
        Assert.Equal(updateDto.Description, updatedCourse.Description);
        Assert.Equal(updateDto.Schedule, updatedCourse.Schedule);
        Assert.Equal(updateDto.Professor, updatedCourse.Professor);
        Assert.NotNull(updatedCourse.ImageUrl); //<- Image URL should not be updated
    }

    [Fact]
    public async Task UpdateCourse_InvalidSchedule_ReturnsBadRequest()
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

        // Invalid update DTO
        var updateDto = new UpdateCourseDto
        {
            Name = "Math Updated",
            Description = "Desc",
            Schedule = "", // Invalid schedule
            Professor = "Prof",
            File = TestHelpers.GetMockFormFile()
        };

        // Act: Call Update with the course's id and the update DTO
        var result = await controller.Update(course.Id, updateDto);

        // Assert: Should return BadRequest
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCourse_InvalidProfessor_ReturnsBadRequest()
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

        // Invalid update DTO
        var updateDto = new UpdateCourseDto
        {
            Name = "Math Updated",
            Description = "Desc",
            Schedule = "Mon",
            Professor = "", // Invalid professor
            File = TestHelpers.GetMockFormFile()
        };

        // Act: Call Update with the course's id and the update DTO
        var result = await controller.Update(course.Id, updateDto);

        // Assert: Should return BadRequest
        Assert.IsType<BadRequestObjectResult>(result);
    }
}