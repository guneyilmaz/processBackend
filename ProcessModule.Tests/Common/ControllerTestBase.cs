using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ProcessModule.Tests.Common;

/// <summary>
/// Base class for controller unit tests providing common setup and utilities
/// </summary>
public abstract class ControllerTestBase<TController> where TController : ControllerBase
{
    protected Mock<ILogger<TController>> MockLogger { get; }
    protected TController Controller { get; set; }

    protected ControllerTestBase()
    {
        MockLogger = new Mock<ILogger<TController>>();
    }

    /// <summary>
    /// Verifies that the controller action returns OkObjectResult with the expected data
    /// </summary>
    protected static void AssertOkResult<T>(ActionResult<T> result, T expectedData)
    {
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedData, okResult.Value);
    }

    /// <summary>
    /// Verifies that the controller action returns BadRequestObjectResult
    /// </summary>
    protected static void AssertBadRequestResult<T>(ActionResult<T> result)
    {
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    /// <summary>
    /// Verifies that the controller action returns NotFoundResult
    /// </summary>
    protected static void AssertNotFoundResult<T>(ActionResult<T> result)
    {
        Assert.IsType<NotFoundResult>(result.Result);
    }

    /// <summary>
    /// Verifies that the controller action returns ObjectResult with status code 500
    /// </summary>
    protected static void AssertInternalServerErrorResult<T>(ActionResult<T> result)
    {
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
    }
}