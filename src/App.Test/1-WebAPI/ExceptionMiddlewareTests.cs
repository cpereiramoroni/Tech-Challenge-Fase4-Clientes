using Api.Middleware;
using App.Application.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task PassesThroughUnhandledExceptionAsync()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        mockNext.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(new Exception("Unhandled Exception"));

        var middleware = new ExceptionMiddleware(mockNext.Object);

        var httpContext = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)httpContext.Response.StatusCode);
    }
    [Fact]
    public async Task InvokeAsync_ClientException_ReturnsBadRequest()
    {
        // Arrange
        var middleware = new ExceptionMiddleware((innerHttpContext) =>
        {
            throw new ClientException("Client error occurred");
        });

        var context = new DefaultHttpContext();
        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;

        // Act
        await middleware.InvokeAsync(context);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.Contains("Client error occurred", responseBody);
    }

    [Fact]
    public async Task PassesWithoutExceptionAsync()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        mockNext.Setup(next => next(It.IsAny<HttpContext>()));

        var middleware = new ExceptionMiddleware(mockNext.Object);

        var httpContext = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task HandlesArgumentExceptionAsync()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        mockNext.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(new ArgumentException("Invalid argument"));

        var middleware = new ExceptionMiddleware(mockNext.Object);

        var httpContext = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)httpContext.Response.StatusCode);

    }

    [Fact]
    public async Task HandlesValidationExceptionAsync()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        mockNext.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(new ValidationException("Validation error"));

        var middleware = new ExceptionMiddleware(mockNext.Object);

        var httpContext = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)httpContext.Response.StatusCode);

    }

    [Fact]
    public async Task HandlesOtherExceptionsAsync()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        mockNext.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(new InvalidOperationException("Unexpected operation"));

        var middleware = new ExceptionMiddleware(mockNext.Object);

        var httpContext = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)httpContext.Response.StatusCode);
    }
}