using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using Moq;
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
    
    [Test]
    public async Task Map_TaskResultTIsSuccessAndTaskMappingFunction_ReturnsResultSuccessWithMappedValue()
    {
        const string value = "value";
        Task<Result<string>> resultTask = Task.FromResult(Result.Success(value));

        const string newValue = "newValue";
        Task<Result<string>> mappedResultTask = resultTask.Map(stringValue => Task.FromResult(stringValue + newValue));

        Result<string> mappedResult = await mappedResultTask;
        
        Assert.That(mappedResult.IsSuccess, Is.True);
        Assert.That(mappedResult.IsFailure, Is.False);
        Assert.That(mappedResult.Value, Is.EqualTo(value + newValue));
        Assert.That(mappedResult.Errors, Is.Empty);
    }
    
    [Test]
    public async Task Map_TaskResultTIsFailureAndTaskMappingFunction_ReturnsResultFailure()
    {
        Error error = new DomainError("error");
        Task<Result<string>> resultTask = Task.FromResult(Result.Failure<string>(error));

        const string newValue = "newValue";
        Task<Result<string>> mappedResultTask = resultTask.Map(stringValue => Task.FromResult(stringValue + newValue));

        Result<string> mappedResult = await mappedResultTask;
        
        Assert.That(mappedResult.IsSuccess, Is.False);
        Assert.That(mappedResult.IsFailure, Is.True);
        Assert.That(mappedResult.Errors.Contains(error), Is.True);
    }

    [Test]
    public async Task Bind_TaskResultTOutIsFailure_ReturnsResultFailure()
    {
        Error error = new DomainError("error");
        Task<Result> resultTask = Task.FromResult(Result.Failure(error));

        const string newValue = "newValue";
        Task<Result<string>> mappedResultTask = resultTask.Bind(() => Task.FromResult(Result.Success(newValue)));

        Result<string> mappedResult = await mappedResultTask;
        
        Assert.That(mappedResult.IsSuccess, Is.False);
        Assert.That(mappedResult.IsFailure, Is.True);
        Assert.That(mappedResult.Errors.Contains(error), Is.True);
    }

    [Test]
    public async Task Bind_TaskResultTOutIsSuccess_ReturnsResultOfFunc()
    {
        Task<Result> resultTask = Task.FromResult(Result.Success());

        const string newValue = "newValue";
        Task<Result<string>> mappedResultTask = resultTask.Bind(() => Task.FromResult(Result.Success(newValue)));

        Result<string> mappedResult = await mappedResultTask;
        
        Assert.That(mappedResult.IsSuccess, Is.True);
        Assert.That(mappedResult.IsFailure, Is.False);
        Assert.That(mappedResult.Value, Is.EqualTo(newValue));
        Assert.That(mappedResult.Errors, Is.Empty);
    }
    
    [Test]
    public async Task Bind_TaskResultTInAndTOutIsFailure_ReturnsResultFailure()
    {
        Error error = new DomainError("error");
        
        Task<Result<string>> resultTask = Task.FromResult(Result.Failure<string>(error));

        const string newValue = "newValue";
        Task<Result<string>> mappedResultTask = resultTask.Bind(value => Task.FromResult(Result.Success(value + newValue)));

        Result<string> mappedResult = await mappedResultTask;
        
        Assert.That(mappedResult.IsSuccess, Is.False);
        Assert.That(mappedResult.IsFailure, Is.True);
        Assert.That(mappedResult.Errors.Contains(error), Is.True);  
    }

    [Test]
    public async Task Bind_TaskResultTInAndTOutIsSuccess_ReturnsResultOfFunc()
    {
        const string oldValue = "oldValue";
        Task<Result<string>> resultTask = Task.FromResult(Result.Success<string>(oldValue));

        const string newValue = "newValue";
        Task<Result<string>> mappedResultTask = resultTask.Bind(value => Task.FromResult(Result.Success(value + newValue)));

        Result<string> mappedResult = await mappedResultTask;
        
        Assert.That(mappedResult.IsSuccess, Is.True);
        Assert.That(mappedResult.IsFailure, Is.False);
        Assert.That(mappedResult.Value, Is.EqualTo(oldValue + newValue));
        Assert.That(mappedResult.Errors, Is.Empty);  
    }

    [Test]
    public async Task Match_ResultIsSuccess_ReturnsResultOfSuccessFunc()
    {
        const string value = "value";
        Task<Result<string>> resultTask = Task.FromResult(Result.Success(value));

        Error error = new DomainError("new error");
        const string newValue = "newValue";
        Result<string> result = await resultTask.Match(
            value => Task.FromResult(Result.Success(value+newValue)),
            () => Task.FromResult(Result.Failure<string>(error)));
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.IsFailure, Is.False);
        Assert.That(result.Value, Is.EqualTo(value + newValue));
        Assert.That(result.Errors, Is.Empty);  
    }
    
    [Test]
    public async Task Match_ResultIsSuccess_ReturnsResultOfFailureFunc()
    {
        Error error = new DomainError("error");
        Task<Result<string>> resultTask = Task.FromResult(Result.Failure<string>(error));

        Error error2 = new DomainError("new error");
        Result<string> result = await resultTask.Match(
            value => Task.FromResult(Result.Success("new Value")),
            () => Task.FromResult(Result.Failure<string>(error2)));
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error2), Is.True);  
    }

    [Test]
    public void Tap_ResultIsFailure_ReturnsResultFailure()
    {
        Error error = new DomainError("error");
        Result<string> result = Result.Failure<string>(error);

        Mock<ISummonerRepository> summonerRepositoryMock = new();
        Result<string> finalResult = result.Tap(value => summonerRepositoryMock.Object.GetByPuuidAsync(value));
        
        Assert.That(finalResult.IsSuccess, Is.False);
        Assert.That(finalResult.IsFailure, Is.True);
        Assert.That(finalResult.Errors.Contains(error), Is.True);  
        summonerRepositoryMock.Verify(x => x.GetByPuuidAsync(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void Tap_ResultIsSuccess_ReturnsResultSuccessAndAwaitsFuncWithValueFromResult()
    {
        const string newValue = "newValue";
        Result<string> result = Result.Success<string>(newValue);

        Mock<ISummonerRepository> summonerRepositoryMock = new();
        Result<string> finalResult = result.Tap(value => summonerRepositoryMock.Object.GetByPuuidAsync(value));
        
        Assert.That(finalResult.IsSuccess, Is.True);
        Assert.That(finalResult.IsFailure, Is.False);
        Assert.That(finalResult.Errors, Is.Empty);  
        summonerRepositoryMock.Verify(x => x.GetByPuuidAsync(newValue), Times.Once);
    }
    
    [Test]
    public async Task Tap_ResultTInIsFailure_ReturnsResultFailureWithoutUsingResultValue()
    {
        Error error = new DomainError("error");
        Task<Result<string>> result = Task.FromResult(Result.Failure<string>(error));

        Mock<ISummonerRepository> summonerRepositoryMock = new();
        Result<string> finalResult = await result.Tap(value => summonerRepositoryMock.Object.GetByPuuidAsync(value));
        
        
        Assert.That(finalResult.IsSuccess, Is.False);
        Assert.That(finalResult.IsFailure, Is.True);
        Assert.That(finalResult.Errors.Contains(error), Is.True);  
        summonerRepositoryMock.Verify(x => x.GetByPuuidAsync(It.IsAny<string>()), Times.Never);
    }
    
    [Test]
    public async Task Tap_ResultTInIsSuccess_ReturnsResultSuccessAndAwaitsFuncWithValueFromResult()
    {
        const string newValue = "newValue";
        Task<Result<string>> result = Task.FromResult(Result.Success<string>(newValue));

        Mock<ISummonerRepository> summonerRepositoryMock = new();
        Result<string> finalResult = await result.Tap(value => summonerRepositoryMock.Object.GetByPuuidAsync(value));
        
        
        Assert.That(finalResult.IsSuccess, Is.True);
        Assert.That(finalResult.IsFailure, Is.False);
        Assert.That(finalResult.Errors, Is.Empty);  
        summonerRepositoryMock.Verify(x => x.GetByPuuidAsync(newValue), Times.Once);
    }
}