using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using api.Controllers;
using api.Data;
using api.Dtos.Course;
using Moq;

namespace tests;

public class CreateCourseTest
{
    private static ApplicationDBContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;
        return new ApplicationDBContext(options);
    }

    private static CourseController GetController(ApplicationDBContext context)
    {
        var envMock = new Mock<IWebHostEnvironment>();
        envMock.Setup(e => e.WebRootPath).Returns(Path.GetTempPath());
        return new CourseController(context, envMock.Object);
    }

    private static IFormFile GetMockFormFile()
    {
        var fileMock = new Mock<IFormFile>();
        var content = "Fake file content";
        var fileName = "test.png";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);
        fileMock.Setup(_ => _.ContentType).Returns("image/png");
        return fileMock.Object;
    }

    [Fact]
    public async Task CreateCourse_HappyPath_ReturnsCreatedCourse()
    {
        // Arrange
        var context = GetDbContext();
        var controller = GetController(context);
        var dto = new CreateCourseDto
        {
            Name = "Mathematics",
            Description = "Basic math course",
            File = GetMockFormFile(),
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
        var context = GetDbContext();
        var controller = GetController(context);
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
        var context = GetDbContext();
        var controller = GetController(context);
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
        var context = GetDbContext();
        var controller = GetController(context);
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
        var context = GetDbContext();
        var controller = GetController(context);
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
        var context = GetDbContext();
        var controller = GetController(context);
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