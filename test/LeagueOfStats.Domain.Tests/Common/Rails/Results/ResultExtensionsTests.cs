using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Common.Rails.Results;

[TestFixture]
public class ResultExtensionsTests
{
    [Test]
    public void ToNonValueResult_ResultTIsSuccess_ReturnsResultSuccess()
    {
        const string value = "value";
        Result<string> result = Result.Success(value);

        Result nonValueResult = result.ToNonValueResult();
        
        Assert.That(nonValueResult, Is.TypeOf<Result>());
        Assert.That(nonValueResult.IsSuccess, Is.True);
        Assert.That(nonValueResult.IsFailure, Is.False);
        Assert.That(nonValueResult.Errors, Is.Empty);
    }
    
    [Test]
    public void ToNonValueResult_ResultTIsFailure_ReturnsResultFailure()
    {
        Error error = new DomainError("error");
        Result<string> result = Result.Failure<string>(error);

        Result nonValueResult = result.ToNonValueResult();
        
        Assert.That(nonValueResult, Is.TypeOf<Result>());
        Assert.That(nonValueResult.IsSuccess, Is.False);
        Assert.That(nonValueResult.IsFailure, Is.True);
        Assert.That(nonValueResult.Errors.Contains(error), Is.True);
    }
    
    [Test]
    public async Task ToNonValueResult_TaskResultTIsSuccess_ReturnsResultSuccess()
    {
        const string value = "value";
        Task<Result<string>> resultTask = Task.FromResult(Result.Success(value));

        Task<Result>nonValueResultTask = resultTask.ToNonValueResult();

        Result nonValueResult = await nonValueResultTask;
        
        Assert.That(nonValueResult, Is.TypeOf<Result>());
        Assert.That(nonValueResult.IsSuccess, Is.True);
        Assert.That(nonValueResult.IsFailure, Is.False);
        Assert.That(nonValueResult.Errors, Is.Empty);
    }
    
    [Test]
    public async Task ToNonValueResult_TaskResultTIsFailure_ReturnsResultFailure()
    {
        Error error = new DomainError("error");
        Task<Result<string>> resultTask = Task.FromResult(Result.Failure<string>(error));

        Task<Result>nonValueResultTask = resultTask.ToNonValueResult();
        
        Result nonValueResult = await nonValueResultTask;
        
        Assert.That(nonValueResult, Is.TypeOf<Result>());
        Assert.That(nonValueResult.IsSuccess, Is.False);
        Assert.That(nonValueResult.IsFailure, Is.True);
        Assert.That(nonValueResult.Errors.Contains(error), Is.True);
    }
    
    [Test]
    public void Map_ResultTIsSuccess_ReturnsResultSuccessWithMappedValue()
    {
        const string value = "value";
        Result<string> result = Result.Success(value);

        const string newValue = "newValue";
        Result<string> mappedResult = result.Map(stringValue => stringValue + newValue);
        
        Assert.That(mappedResult.IsSuccess, Is.True);
        Assert.That(mappedResult.IsFailure, Is.False);
        Assert.That(mappedResult.Value, Is.EqualTo(value + newValue));
        Assert.That(mappedResult.Errors, Is.Empty);
    }
    
    [Test]
    public void Map_ResultTIsFailure_ReturnsResultFailure()
    {
        Error error = new DomainError("error");
        Result<string> result = Result.Failure<string>(error);

        const string newValue = "newValue";
        Result<string> mappedResult = result.Map(stringValue => stringValue + newValue);
        
        Assert.That(mappedResult.IsSuccess, Is.False);
        Assert.That(mappedResult.IsFailure, Is.True);
        Assert.That(mappedResult.Errors.Contains(error), Is.True);
    }
    
    [Test]
    public async Task Map_TaskResultTIsSuccess_ReturnsResultSuccessWithMappedValue()
    {
        const string value = "value";
        Task<Result<string>> resultTask = Task.FromResult(Result.Success(value));

        const string newValue = "newValue";
        Task<Result<string>> mappedResultTask = resultTask.Map(stringValue => stringValue + newValue);

        Result<string> mappedResult = await mappedResultTask;
        
        Assert.That(mappedResult.IsSuccess, Is.True);
        Assert.That(mappedResult.IsFailure, Is.False);
        Assert.That(mappedResult.Value, Is.EqualTo(value + newValue));
        Assert.That(mappedResult.Errors, Is.Empty);
    }
    
    [Test]
    public async Task Map_TaskResultTIsFailure_ReturnsResultFailure()
    {
        Error error = new DomainError("error");
        Task<Result<string>> resultTask = Task.FromResult(Result.Failure<string>(error));

        const string newValue = "newValue";
        Task<Result<string>> mappedResultTask = resultTask.Map(stringValue => stringValue + newValue);

        Result<string> mappedResult = await mappedResultTask;
        
        Assert.That(mappedResult.IsSuccess, Is.False);
        Assert.That(mappedResult.IsFailure, Is.True);
        Assert.That(mappedResult.Errors.Contains(error), Is.True);
    }
}