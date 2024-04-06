using FluentValidation.Results;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.API.Tests.Extensions;

[TestFixture]
public class ResultExtensionsTests
{
    private readonly Mock<ControllerBase> _controllerBasMock = new();

    [SetUp]
    public void SetUp()
    {
        _controllerBasMock.Reset();
    }
    
    [Test]
    public async Task ToIActionResult_NonGenericResultIsSuccess_ReturnsOk()
    {
        Task<Result> result = Task.FromResult(Result.Success());
        
        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.Ok(), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_NonGenericResultIsFailureAndMoreThanOneErrorType_ThrowsNotSupportedException()
    {
        ApiError apiError = new ApiError("error");
        DomainError domainError = new DomainError("error");
        Error[] errors =
        {
            apiError,
            domainError
        };
        Task<Result> result = Task.FromResult(Result.Failure(errors));
        
        Assert.ThrowsAsync<NotSupportedException>(
            () => result.ToIActionResult(_controllerBasMock.Object),
            "Converting errors of more than one type is not yet supported.");
    }
    
    [Test]
    public async Task ToIActionResult_NonGenericResultIsFailureAndErrorTypeIsApiError_ReturnsBadRequestWithErrorMessage()
    {
        ApiError apiError = new ApiError("error");
        Task<Result> result = Task.FromResult(Result.Failure(apiError));

        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.BadRequest(apiError.ErrorMessage), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_NonGenericResultIsFailureAndErrorTypeIsApplicationError_ReturnsBadRequestWithErrorMessage()
    {
        ApplicationError applicationError = new ApplicationError("error");
        Task<Result> result = Task.FromResult(Result.Failure(applicationError));

        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.BadRequest(applicationError.ErrorMessage), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_NonGenericResultIsFailureAndErrorTypeIsDomainError_ReturnsBadRequestWithErrorMessage()
    {
        DomainError domainError= new DomainError("error");
        Task<Result> result = Task.FromResult(Result.Failure(domainError));

        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.BadRequest(domainError.ErrorMessage), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_NonGenericResultIsFailureAndErrorTypeIsEntityNotFoundError_ReturnsNotFoundWithErrorMessage()
    {
        EntityNotFoundError entityNotFoundError = new EntityNotFoundError("error");
        Task<Result> result = Task.FromResult(Result.Failure(entityNotFoundError));

        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.NotFound(entityNotFoundError.ErrorMessage), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_NonGenericResultIsFailureAndErrorTypeIsValidationError_ReturnsBadRequestWithErrorMessage()
    {
        ValidationError validationError = new ValidationError(new ValidationFailure(default, "error"));
        Task<Result> result = Task.FromResult(Result.Failure(validationError));

        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.BadRequest(validationError.ErrorMessage), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_NonGenericResultIsFailureAndErrorIsNotSupported_ThrowsNotSupportedException()
    {
        ResultExtensionsTestsError resultExtensionsTestsError = new ResultExtensionsTestsError("error");
        Task<Result> result = Task.FromResult(Result.Failure(resultExtensionsTestsError));
        
        Assert.ThrowsAsync<NotSupportedException>(
            () => result.ToIActionResult(_controllerBasMock.Object),
            "This error is not yet supported.");
    }
    
    [Test]
    public async Task ToIActionResult_GenericResultIsSuccess_ReturnsOkWithResultValue()
    {
        const string value = "value";
        Task<Result<string>> result = Task.FromResult(Result.Success(value));
        
        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.Ok(value), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_GenericResultIsFailureAndMoreThanOneErrorType_ThrowsNotSupportedException()
    {
        ApiError apiError = new ApiError("error");
        Task<Result<string>> result = Task.FromResult(Result.Failure<string>(apiError));

        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.BadRequest(apiError.ErrorMessage), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_GenericResultIsFailureAndErrorTypeIsApiError_ReturnsBadRequestWithErrorMessage()
    {
        ApiError apiError = new ApiError("error");
        Task<Result<string>> result = Task.FromResult(Result.Failure<string>(apiError));

        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.BadRequest(apiError.ErrorMessage), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_GenericResultIsFailureAndErrorTypeIsApplicationError_ReturnsBadRequestWithErrorMessage()
    {
        ApplicationError applicationError = new ApplicationError("error");
        Task<Result<string>> result = Task.FromResult(Result.Failure<string>(applicationError));

        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.BadRequest(applicationError.ErrorMessage), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_GenericResultIsFailureAndErrorTypeIsDomainError_ReturnsBadRequestWithErrorMessage()
    {
        DomainError domainError= new DomainError("error");
        Task<Result<string>> result = Task.FromResult(Result.Failure<string>(domainError));

        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.BadRequest(domainError.ErrorMessage), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_GenericResultIsFailureAndErrorTypeIsEntityNotFoundError_ReturnsNotFoundWithErrorMessage()
    {
        EntityNotFoundError entityNotFoundError = new EntityNotFoundError("error");
        Task<Result<string>> result = Task.FromResult(Result.Failure<string>(entityNotFoundError));

        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.NotFound(entityNotFoundError.ErrorMessage), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_GenericResultIsFailureAndErrorTypeIsValidationError_ReturnsBadRequestWithErrorMessage()
    {
        ValidationError validationError = new ValidationError(new ValidationFailure(default, "error"));
        Task<Result<string>> result = Task.FromResult(Result.Failure<string>(validationError));

        await result.ToIActionResult(_controllerBasMock.Object);
        
        _controllerBasMock.Verify(x => x.BadRequest(validationError.ErrorMessage), Times.Once);
    }
    
    [Test]
    public async Task ToIActionResult_GenericResultIsFailureAndErrorIsNotSupported_ThrowsNotSupportedException()
    {
        ResultExtensionsTestsError resultExtensionsTestsError = new ResultExtensionsTestsError("error");
        Task<Result<string>> result = Task.FromResult(Result.Failure<string>(resultExtensionsTestsError));
        
        Assert.ThrowsAsync<NotSupportedException>(
            () => result.ToIActionResult(_controllerBasMock.Object),
            "This error is not yet supported.");
    }

    private sealed class ResultExtensionsTestsError : Error
    {
        public ResultExtensionsTestsError(string errorMessage) : base(errorMessage)
        {
        }
    }
}
