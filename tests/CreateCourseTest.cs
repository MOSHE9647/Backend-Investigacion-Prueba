using Microsoft.AspNetCore.Mvc;
using api.Dtos.Course;
using api.Data;
using api.Controllers;

namespace tests;

public class CreateCourseTest
{
    private readonly ApplicationDBContext context;
    private readonly CourseController controller;

    public CreateCourseTest()
    {
        context = TestHelpers.GetDbContext();
        controller = TestHelpers.GetController(context);
    }

    [Fact]
    public async Task CreateCourse_HappyPath_ReturnsCreatedCourse()
    {
        // Arrange: Create a valid CreateCourseDto
        // This DTO will be used to create a new course
        var dto = new CreateCourseDto
        {
            Name = "Mathematics",
            Description = "Basic math course",
            File = TestHelpers.GetMockFormFile(),
            Schedule = "Monday, Wednesday, Friday",
            Professor = "Dr. Smith",
        };

        // Act: Call Create with the DTO
        var result = await controller.Create(dto);

        // Assert: Should return CreatedAtAction and the course should be created
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var course = Assert.IsType<CourseDto>(createdResult.Value);
        Assert.Equal(dto.Name, course.Name);
        Assert.Equal(dto.Description, course.Description);
        Assert.Equal(dto.Schedule, course.Schedule);
        Assert.Equal(dto.Professor, course.Professor);
        Assert.NotNull(course.ImageUrl);
    }

    [Fact]
    public async Task CreateCourse_InvalidName_ReturnsBadRequest()
    {
        // Arrange: Create a DTO with an invalid name
        var dto = new CreateCourseDto
        {
            Name = "", // Invalid name
            Description = "Some description",
            File = TestHelpers.GetMockFormFile(),
            Schedule = "Monday, Wednesday, Friday",
            Professor = "Dr. Smith",
        };

        // Act: Call Create with the DTO
        var result = await controller.Create(dto);

        // Assert: Should return BadRequest
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCourse_InvalidDescription_ReturnsBadRequest()
    {
        // Arrange: Create a DTO with an invalid description
        var dto = new CreateCourseDto
        {
            Name = "Physics",
            Description = "", // Invalid description
            File = TestHelpers.GetMockFormFile(),
            Schedule = "Monday, Wednesday, Friday",
            Professor = "Dr. Smith",
        };

        // Act: Call Create with the DTO
        var result = await controller.Create(dto);

        // Assert: Should return BadRequest
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCourse_InvalidFile_ReturnsBadRequest()
    {
        // Arrange: Create a DTO with an invalid file
        var dto = new CreateCourseDto
        {
            Name = "Chemistry",
            Description = "Chemistry course",
            File = null, // Invalid file
            Schedule = "Monday, Wednesday, Friday",
            Professor = "Dr. Smith",
        };

        // Act: Call Create with the DTO
        var result = await controller.Create(dto);

        // Assert: Should return BadRequest
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCourse_InvalidSchedule_ReturnsBadRequest()
    {
        // Arrange: Create a DTO with an invalid schedule
        var dto = new CreateCourseDto
        {
            Name = "Chemistry",
            Description = "Chemistry course",
            File = TestHelpers.GetMockFormFile(),
            Schedule = "", // Invalid schedule
            Professor = "Dr. Smith",
        };

        // Act: Call Create with the DTO
        var result = await controller.Create(dto);

        // Assert: Should return BadRequest
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCourse_InvalidProfessor_ReturnsBadRequest()
    {
        // Arrange: Create a DTO with an invalid professor
        var dto = new CreateCourseDto
        {
            Name = "Biology",
            Description = "Biology course",
            File = TestHelpers.GetMockFormFile(),
            Schedule = "Tuesday, Thursday",
            Professor = "", // Invalid professor
        };

        // Act: Call Create with the DTO
        var result = await controller.Create(dto);

        // Assert: Should return BadRequest
        Assert.IsType<BadRequestObjectResult>(result);
    }
}