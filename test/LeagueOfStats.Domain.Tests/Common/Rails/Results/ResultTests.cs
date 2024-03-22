using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Common.Rails.Results;

[TestFixture]
public class ResultTests
{
    [Test]
    public void Constructor_IsSuccessAndErrorIsNotNull_ThrowsInvalidOperationException()
    {
        const bool isSuccess = true;
        Error? error = new DomainError("error");
        
        Assert.Throws<InvalidOperationException>(() => new Result(isSuccess, error));
    }
    
    [Test]
    public void Constructor_IsSuccessIsFalseAndErrorIsNull_ThrowsInvalidOperationException()
    {
        const bool isSuccess = false;
        Error? error = null;
        
        Assert.Throws<InvalidOperationException>(() => new Result(isSuccess, error));
    }
    
    [Test]
    public void Constructor_IsSuccessAndErrorsIsNotNull_ThrowsInvalidOperationException()
    {
        const bool isSuccess = true;
        Error[]? errors = { };
        
        Assert.Throws<InvalidOperationException>(() => new Result(isSuccess, errors));
    }
    
    [Test]
    public void Constructor_IsSuccessIsFalseAndErrorsIsNull_ThrowsInvalidOperationException()
    {
        const bool isSuccess = false;
        Error[]? errors = null;
        
        Assert.Throws<InvalidOperationException>(() => new Result(isSuccess, errors));
    }
    
    [Test]
    public void Constructor_IsSuccessIsFalseAndErrorsIsEmpty_ThrowsInvalidOperationException()
    {
        const bool isSuccess = false;
        Error[]? errors = { };
        
        Assert.Throws<InvalidOperationException>(() => new Result(isSuccess, errors));
    }

    [Test]
    public void Constructor_IsSuccessIsTrueAndErrorIsNull_ReturnsResultWithSuccess()
    {
        const bool isSuccess = true;
        Error? error = null;

        Result result = new Result(isSuccess, error);
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.IsFailure, Is.False);
        Assert.That(result.Errors, Is.Empty);
    }
    
    [Test]
    public void Constructor_IsSuccessIsFalseAndErrorIsNotNull_ReturnsResultWithFailure()
    {
        const bool isSuccess = false;
        Error error = new DomainError("error1");

        Result result = new Result(isSuccess, error);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error), Is.True);
    }
    
    [Test]
    public void Constructor_IsSuccessIsFalseAndErrorsAreNotNullAndNotEmpty_ReturnsResultWithFailure()
    {
        const bool isSuccess = false;
        Error error1 = new DomainError("error1");
        Error error2 = new DomainError("error2");
        Error[]? errors =
        {
            error1,
            error2
        };

        Result result = new Result(isSuccess, errors);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors, Is.EqualTo(errors));
    }
    
        [Test]
    public void Constructor_IsGenericAndIsSuccessAndErrorIsNotNull_ThrowsInvalidOperationException()
    {
        const string value = "value";
        const bool isSuccess = true;
        Error? error = new DomainError("error");
        
        Assert.Throws<InvalidOperationException>(() => new Result<string>(value, isSuccess, error));
    }
    
    [Test]
    public void Constructor_IsGenericAndIsSuccessIsFalseAndErrorIsNull_ThrowsInvalidOperationException()
    {
        const string value = "value";
        const bool isSuccess = false;
        Error? error = null;
        
        Assert.Throws<InvalidOperationException>(() => new Result<string>(value, isSuccess, error));
    }
    
    [Test]
    public void Constructor_IsGenericAndIsSuccessAndErrorsIsNotNull_ThrowsInvalidOperationException()
    {
        const string value = "value";
        const bool isSuccess = true;
        Error[]? errors = { };
        
        Assert.Throws<InvalidOperationException>(() => new Result<string>(value, isSuccess, errors));
    }
    
    [Test]
    public void Constructor_IsGenericAndIsSuccessIsFalseAndErrorsIsNull_ThrowsInvalidOperationException()
    {
        const string value = "value";
        const bool isSuccess = false;
        Error[]? errors = null;
        
        Assert.Throws<InvalidOperationException>(() => new Result<string>(value, isSuccess, errors));
    }
    
    [Test]
    public void Constructor_IsGenericAndIsSuccessIsFalseAndErrorsIsEmpty_ThrowsInvalidOperationException()
    {
        const string value = "value";
        const bool isSuccess = false;
        Error[]? errors = { };
        
        Assert.Throws<InvalidOperationException>(() => new Result<string>(value, isSuccess, errors));
    }

    [Test]
    public void Constructor_IsGenericAndIsSuccessIsTrueAndErrorIsNull_ReturnsResultWithSuccessAndValue()
    {
        const string value = "value";
        const bool isSuccess = true;
        Error? error = null;

        Result<string> result = new Result<string>(value, isSuccess, error);
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.IsFailure, Is.False);
        Assert.That(result.Value, Is.EqualTo(value));
        Assert.That(result.Errors, Is.Empty);
    }
    
    [Test]
    public void Constructor_IsGenericAndIsSuccessIsFalseAndErrorIsNotNull_ReturnsResultWithFailure()
    {
        const string value = "value";
        const bool isSuccess = false;
        Error error = new DomainError("error1");

        Result<string> result = new Result<string>(value, isSuccess, error);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error), Is.True);
    }
    
    [Test]
    public void Constructor_IsGenericAndIsSuccessIsFalseAndErrorsAreNotNullAndNotEmpty_ReturnsResultWithFailure()
    {
        const string value = "value";
        const bool isSuccess = false;
        Error error1 = new DomainError("error1");
        Error error2 = new DomainError("error2");
        Error[]? errors =
        {
            error1,
            error2
        };

        Result<string> result = new Result<string>(value, isSuccess, errors);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors, Is.EqualTo(errors));
    }

    [Test]
    public void AggregatedErrorMessages_AllValid_JoinsErrorsByNewLines()
    {
        const bool isSuccess = false;
        Error error1 = new DomainError("error1");
        Error error2 = new DomainError("error2");
        Error[]? errors =
        {
            error1,
            error2
        };

        Result result = new Result(isSuccess, errors);

        string aggregatedErrorMessages = result.AggregatedErrorMessages;
        
        Assert.That(aggregatedErrorMessages, Is.EqualTo(string.Join(Environment.NewLine, errors.Select(e => e.ErrorMessage))));
    }

    [Test]
    public void Success_NonGeneric_ReturnsResultWithSuccessTrueAndErrorNull()
    {
        Result result = Result.Success();

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.IsFailure, Is.False);
        Assert.That(result.Errors, Is.Empty);
    }
    
    [Test]
    public void Success_Generic_ReturnsResultWithSuccessTrueAndTValueAndErrorNull()
    {
        const string value = "value";
        Result<string> result = Result.Success(value);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.IsFailure, Is.False);
        Assert.That(result.Value, Is.EqualTo(value));
        Assert.That(result.Errors, Is.Empty);
    }
    
    [Test]
    public void Failure_NonGenericAndError_ReturnsResultWithSuccessFalseAndError()
    {
        Error error = new DomainError("error");
        Result result = Result.Failure(error);

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error), Is.True);
    }
    
    [Test]
    public void Failure_NonGenericAndErrors_ReturnsResultWithSuccessFalseAndErrors()
    {
        Error error1 = new DomainError("error1");
        Error error2 = new DomainError("error2");
        Error[] errors =
        {
            error1,
            error2
        };
        
        Result result = Result.Failure(errors);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors, Is.EqualTo(errors));
    }
    
    [Test]
    public void Failure_Generic_ReturnsResultWithSuccessFalseAndDefaultTValueAndError()
    {
        Error error = new DomainError("error");
        Result<string> result = Result.Failure<string>(error);

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error), Is.True);
    }
    
    [Test]
    public void Failure_Generic_ReturnsResultWithSuccessFalseAndDefaultTValueAndErrors()
    {
        Error error1 = new DomainError("error1");
        Error error2 = new DomainError("error2");
        Error[] errors =
        {
            error1,
            error2
        };
        
        Result<string> result = Result.Failure<string>(errors);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors, Is.EqualTo(errors));
    }

    [Test]
    public void Ensure_PredicateIsTrue_ReturnsResultSuccessWithValue()
    {
        const string value = "value";
        Func<string, bool> predicate = stringValue => true;
        Error error = new DomainError("error");

        Result<string> result = Result.Ensure(value, predicate, error);
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.IsFailure, Is.False);
        Assert.That(result.Value, Is.EqualTo(value));
        Assert.That(result.Errors, Is.Empty);
    }
    
    [Test]
    public void Ensure_PredicateIsFalse_ReturnsResultFailureWithError()
    {
        const string value = "value";
        Func<string, bool> predicate = stringValue => false;
        Error error = new DomainError("error");

        Result result = Result.Ensure(value, predicate, error);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error), Is.True);
    }
    
    [Test]
    public void Ensure_AllPredicatesAreTrue_ReturnsResultSuccessWithValue()
    {
        const string value = "value";
        Func<string, bool> predicate1 = stringValue => true;
        Func<string, bool> predicate2 = stringValue => true;
        Error error1 = new DomainError("error");
        Error error2 = new DomainError("error");

        Result<string> result = Result.Ensure(
            value, 
            (predicate1, error1),
            (predicate2, error2));
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.IsFailure, Is.False);
        Assert.That(result.Value, Is.EqualTo(value));
        Assert.That(result.Errors, Is.Empty);
    }
    
    [Test]
    public void Ensure_SomePredicateAsreFalse_ReturnsResultFailureWithFailedErrors()
    {
        const string value = "value";
        Func<string, bool> predicate1 = stringValue => true;
        Func<string, bool> predicate2 = stringValue => false;
        Error error1 = new DomainError("error");
        Error error2 = new DomainError("error");

        Result result = Result.Ensure(
            value, 
            (predicate1, error1),
            (predicate2, error2));
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error2), Is.True);
    }
    
    [Test]
    public void Ensure_AllPredicatesAreFalse_ReturnsResultFailureWithAllErrors()
    {
        const string value = "value";
        Func<string, bool> predicate1 = stringValue => false;
        Func<string, bool> predicate2 = stringValue => false;
        Error error1 = new DomainError("error");
        Error error2 = new DomainError("error");

        Result result = Result.Ensure(
            value, 
            (predicate1, error1),
            (predicate2, error2));
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error1), Is.True); 
        Assert.That(result.Errors.Contains(error2), Is.True);
    }

    [Test]
    public void Combine_SomeResultsHasFailure_ReturnsResultFailureWithFailedResultErrors()
    {
        const string value = "value";
        Error error = new DomainError("error");
        Result<string> failedResult = Result.Failure<string>(error);
        Result<string> succeededResult = Result.Success(value);

        Result<string> result = Result.Combine<string>(failedResult, succeededResult);
        
                
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error), Is.True); 
    }
    
    [Test]
    public void Combine_AllResultsHasFailure_ReturnsResultFailureWithAllFailedResultErrors()
    {
        Error error1 = new DomainError("error");
        Error error2 = new DomainError("error");
        Result<string> failedResult1 = Result.Failure<string>(error1);
        Result<string> failedResult2 = Result.Failure<string>(error2);

        Result<string> result = Result.Combine<string>(failedResult1, failedResult2);
        
                
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error1), Is.True); 
        Assert.That(result.Errors.Contains(error2), Is.True); 
    }
    
    [Test]
    public void Combine_AllResultsAreSuccess_ReturnsResultSuccessWithValue()
    {
        const string value = "value";
        Result<string> failedResult1 = Result.Success(value);
        Result<string> failedResult2 = Result.Success(value);

        Result<string> result = Result.Combine<string>(failedResult1, failedResult2);
        
                
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.IsFailure, Is.False);
        Assert.That(result.Value, Is.EqualTo(value));
        Assert.That(result.Errors, Is.Empty); 
    }

    [Test]
    public void Combine_Result1IsFailure_ReturnsResultFailureWithResult1Error()
    {
        const int value = 1;
        Error error = new DomainError("error");
        Result<string> failedResult = Result.Failure<string>(error);
        Result<int> succeededResult = Result.Success(value);

        Result<(string, int)> result = Result.Combine(failedResult, succeededResult);
        
                
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error), Is.True); 
    }
    
    [Test]
    public void Combine_Result2IsFailure_ReturnsResultFailureWithResult2Error()
    {
        const string value = "value";
        Error error = new DomainError("error");
        Result<string> succeededResult = Result.Success(value);
        Result<int> failedResult = Result.Failure<int>(error);

        Result<(string, int)> result = Result.Combine(succeededResult, failedResult);
        
                
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Contains(error), Is.True); 
    }

    [Test]
    public void Combine_Result1IsSuccessAndResult2IsSuccess_ReturnsResultSuccessWithTupledValues()
    {
        const string value1 = "value";
        const int value2 = 1;
        Result<string> succeededResult1 = Result.Success(value1);
        Result<int> succeededResult2 = Result.Success(value2);

        Result<(string, int)> result = Result.Combine(succeededResult1, succeededResult2);
        
                
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.IsFailure, Is.False);
        Assert.That(result.Value, Is.EqualTo((value1, value2)));
        Assert.That(result.Errors, Is.Empty); 
    }

    [Test]
    public void Value_ResultIsSuccessWithValue_ReturnsValue()
    {
        const string value = "value";
        const bool isSuccess = true;
        Error? error = null;
        Result<string> result = new Result<string>(value, isSuccess, error);
        
        Assert.That(result.Value, Is.EqualTo(value));
    }

    [Test]
    public void Value_ResultIsFailure_ThrowsInvalidOperationException()
    {
        const string value = "value";
        string notAccessibleValue;
        const bool isSuccess = false;
        Error? error = new DomainError("error");
        Result<string> result = new Result<string>(value, isSuccess, error);

        Assert.Throws<InvalidOperationException>(() => notAccessibleValue = result.Value);
    }
}