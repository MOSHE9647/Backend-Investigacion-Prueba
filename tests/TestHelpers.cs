using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using api.Controllers;
using api.Data;

namespace tests;

public static class TestHelpers
{
    public static ApplicationDBContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;
        return new ApplicationDBContext(options);
    }

    public static CourseController GetController(ApplicationDBContext context)
    {
        var envMock = new Mock<IWebHostEnvironment>();
        envMock.Setup(e => e.WebRootPath).Returns(Path.GetTempPath());
        return new CourseController(context, envMock.Object);
    }

    public static IFormFile GetMockFormFile()
    {
        // Create a mock IFormFile with some fake content
        var fileMock = new Mock<IFormFile>();
        var content = "Fake file content";
        var fileName = "testImage_" + Guid.NewGuid() + ".png";
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
}