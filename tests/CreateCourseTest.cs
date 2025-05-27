using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using api.Dtos.Course;
using Moq;

namespace tests;

public class CreateCourseTest
{
    [Fact]
    public async Task CreateCourse_HappyPath_ReturnsCreatedCourse()
    {
        // Arrange
        var context = TestHelpers.GetDbContext();
        var controller = TestHelpers.GetController(context);
        var dto = new CreateCourseDto
        {
            Name = "Mathematics",
            Description = "Basic math course",
            File = TestHelpers.GetMockFormFile(),
            Schedule = "Monday, Wednesday, Friday",
            Professor = "Dr. Smith",
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
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
        // Arrange
        var context = TestHelpers.GetDbContext();
        var controller = TestHelpers.GetController(context);
        var dto = new CreateCourseDto
        {
            Name = "", // Invalid name
            Description = "Some description",
            File = new Mock<IFormFile>().Object,
            Schedule = "Monday, Wednesday, Friday",
            Professor = "Dr. Smith",
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCourse_InvalidDescription_ReturnsBadRequest()
    {
        // Arrange
        var context = TestHelpers.GetDbContext();
        var controller = TestHelpers.GetController(context);
        var dto = new CreateCourseDto
        {
            Name = "Physics",
            Description = "", // Invalid description
            File = new Mock<IFormFile>().Object,
            Schedule = "Monday, Wednesday, Friday",
            Professor = "Dr. Smith",
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCourse_InvalidFile_ReturnsBadRequest()
    {
        // Arrange
        var context = TestHelpers.GetDbContext();
        var controller = TestHelpers.GetController(context);
        var dto = new CreateCourseDto
        {
            Name = "Chemistry",
            Description = "Chemistry course",
            File = null, // Invalid file
            Schedule = "Monday, Wednesday, Friday",
            Professor = "Dr. Smith",
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCourse_InvalidSchedule_ReturnsBadRequest()
    {
        // Arrange
        var context = TestHelpers.GetDbContext();
        var controller = TestHelpers.GetController(context);
        var dto = new CreateCourseDto
        {
            Name = "Chemistry",
            Description = "Chemistry course",
            File = new Mock<IFormFile>().Object,
            Schedule = "", // Invalid schedule
            Professor = "Dr. Smith",
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCourse_InvalidProfessor_ReturnsBadRequest()
    {
        // Arrange
        var context = TestHelpers.GetDbContext();
        var controller = TestHelpers.GetController(context);
        var dto = new CreateCourseDto
        {
            Name = "Biology",
            Description = "Biology course",
            File = new Mock<IFormFile>().Object,
            Schedule = "Tuesday, Thursday",
            Professor = "", // Invalid professor
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}