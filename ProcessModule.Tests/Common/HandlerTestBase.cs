using Moq;
using Microsoft.Extensions.Logging;

namespace ProcessModule.Tests.Common;

/// <summary>
/// Base class for handler unit tests providing common setup and utilities
/// </summary>
public abstract class HandlerTestBase<THandler> where THandler : class
{
    protected Mock<ILogger<THandler>> MockLogger { get; }
    protected THandler Handler { get; set; } = null!;

    protected HandlerTestBase()
    {
        MockLogger = new Mock<ILogger<THandler>>();
    }

    /// <summary>
    /// Setup method to be called in derived classes after dependencies are configured
    /// </summary>
    protected abstract void SetupHandler();

    /// <summary>
    /// Verifies that the logger was called with the expected log level and message
    /// </summary>
    protected void VerifyLoggerCall(LogLevel logLevel, string expectedMessage, Times times)
    {
        MockLogger.Verify(
            x => x.Log(
                logLevel,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }
}