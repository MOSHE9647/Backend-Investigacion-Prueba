using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Controllers;
using api.Dtos.Course;

namespace tests;

public class ReadCourseTest
{
    private readonly ApplicationDBContext context;
    private readonly CourseController controller;

    public ReadCourseTest()
    {
        context = TestHelpers.GetDbContext();
        controller = TestHelpers.GetController(context);
    }

    [Fact]
    public async Task GetAllCourses_ReturnsAllCourses()
    {
        // Arrange: Add multiple courses to the context
        var courses = new List<CreateCourseDto>
        {
            new() { Name = "Course 1", Description = "Desc 1", Schedule = "Mon", Professor = "Prof 1", File = TestHelpers.GetMockFormFile() },
            new() { Name = "Course 2", Description = "Desc 2", Schedule = "Tue", Professor = "Prof 2", File = TestHelpers.GetMockFormFile() },
            new() { Name = "Course 3", Description = "Desc 3", Schedule = "Wed", Professor = "Prof 3", File = TestHelpers.GetMockFormFile() }
        };
        foreach (var courseDto in courses)
        {
            var createResult = await controller.Create(courseDto);
            var created = Assert.IsType<CreatedAtActionResult>(createResult);
            Assert.IsType<CourseDto>(created.Value);
        }

        // Act: Call GetAll
        var result = await controller.GetAll();

        // Assert: Should return OkObjectResult with all courses
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCourses = Assert.IsType<IEnumerable<CourseDto>>(okResult.Value, exactMatch: false);

        // Check if the number of returned courses matches the number added
        Assert.NotNull(returnedCourses);
        Assert.Equal(3, returnedCourses.Count());

        // Verify each course's properties
        Assert.NotEmpty(returnedCourses);
        foreach (var expected in courses)
        {
            var actual = returnedCourses.FirstOrDefault(c => c.Name == expected.Name);
            Assert.NotNull(actual);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Schedule, actual.Schedule);
            Assert.Equal(expected.Professor, actual.Professor);
            Assert.NotNull(actual.ImageUrl);
        }
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsCourse()
    {
        // Arrange: Add a course to the context
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

        // Act: Call GetById with the course's id
        var result = await controller.GetById(course.Id);

        // Assert: Should return OkObjectResult with the course
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCourse = Assert.IsType<CourseDto>(okResult.Value);
        Assert.Equal(course.Id, returnedCourse.Id);
        Assert.Equal(course.Name, returnedCourse.Name);
        Assert.Equal(course.Description, returnedCourse.Description);
        Assert.Equal(course.Schedule, returnedCourse.Schedule);
        Assert.Equal(course.Professor, returnedCourse.Professor);
        Assert.NotNull(returnedCourse.ImageUrl);
    }

    [Fact]
    public async Task GetById_NonExistentId_ReturnsNotFound()
    {
        // Arrange: Use an id that does not exist
        int nonExistentId = 9999;

        // Act: Call GetById with non-existent id
        var result = await controller.GetById(nonExistentId);

        // Assert: Should return NotFound
        Assert.IsType<NotFoundResult>(result);
    }
}