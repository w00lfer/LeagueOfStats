using FluentValidation;
using FluentValidation.Results;
using LeagueOfStats.Application.Common.Behaviors;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.Tests.Common.TestCommons;
using LeagueOfStats.Domain.Common.Rails.Results;
using MediatR;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Application.Tests.Common.Behaviors;

[TestFixture]
public class ValidationBehaviorTests
{
    [Test]
    public async Task Handle_NoValidators_CallsNext()
    {
        IEnumerable<IValidator<DummyCommand>> validators = Enumerable.Empty<IValidator<DummyCommand>>();
        ValidationBehavior<DummyCommand, Result> validationBehavior = new(validators);
        
        Mock<RequestHandlerDelegate<Result>> requestHandlerDelegateMock = new();
        Result result = Result.Success();
        requestHandlerDelegateMock
            .Setup(x => x())
            .ReturnsAsync(result);
        
        DummyCommand dummyCommand = new();
        Result resultFromPipeline = await validationBehavior.Handle(dummyCommand, requestHandlerDelegateMock.Object, CancellationToken.None);

        Assert.That(resultFromPipeline, Is.EqualTo(result));
        
        requestHandlerDelegateMock.Verify(x => x(), Times.Once);
    }

    [Test]
    public async Task Handle_ValidationErrorsAndIsQuery_ReturnsResultFailureWithValue()
    {
        Mock<IValidator<DummyQuery>> validatorMock = new();
        IEnumerable<IValidator<DummyQuery>> validators = new List<IValidator<DummyQuery>>
        {
            validatorMock.Object
        };
        ValidationBehavior<DummyQuery, Result<DummyValue>> validationBehavior = new(validators);
        
        DummyQuery dummyQuery = new();
        const string errorMessage = "error";
        List<ValidationFailure> validationFailures = new List<ValidationFailure>
        {
            new("propertyName", errorMessage)
        };
        validatorMock
            .Setup(x => x.ValidateAsync(dummyQuery, CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));

                
        Mock<RequestHandlerDelegate<Result<DummyValue>>> requestHandlerDelegateMock = new();
        Result<DummyValue> resultFromPipeline = await validationBehavior.Handle(dummyQuery, requestHandlerDelegateMock.Object, CancellationToken.None);

        Assert.That(resultFromPipeline.IsFailure, Is.True);
        Assert.That(resultFromPipeline.Errors.Single(), Is.TypeOf<ValidationError>());
        Assert.That(resultFromPipeline.Errors.Single().ErrorMessage, Is.EqualTo(errorMessage));
        
        validatorMock.Verify(x => x.ValidateAsync(dummyQuery, CancellationToken.None));
        
        validatorMock.VerifyNoOtherCalls();
        requestHandlerDelegateMock.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Handle_ValidationErrorsAndIsCommand_ReturnsResultFailure()
    {
        Mock<IValidator<DummyCommand>> validatorMock = new();
        IEnumerable<IValidator<DummyCommand>> validators = new List<IValidator<DummyCommand>>
        {
            validatorMock.Object
        };
        ValidationBehavior<DummyCommand, Result> validationBehavior = new(validators);
        
        DummyCommand dummyCommand = new();
        const string errorMessage = "error";
        List<ValidationFailure> validationFailures = new List<ValidationFailure>
        {
            new("propertyName", errorMessage)
        };
        validatorMock
            .Setup(x => x.ValidateAsync(dummyCommand, CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
                
        Mock<RequestHandlerDelegate<Result>> requestHandlerDelegateMock = new();
        Result resultFromPipeline = await validationBehavior.Handle(dummyCommand, requestHandlerDelegateMock.Object, CancellationToken.None);

        Assert.That(resultFromPipeline.IsFailure, Is.True);
        Assert.That(resultFromPipeline.Errors.Single(), Is.TypeOf<ValidationError>());
        Assert.That(resultFromPipeline.Errors.Single().ErrorMessage, Is.EqualTo(errorMessage));
        
        validatorMock.Verify(x => x.ValidateAsync(dummyCommand, CancellationToken.None));
        
        validatorMock.VerifyNoOtherCalls();
        requestHandlerDelegateMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Handle_NoValidationErrors_CallsNext()
    {
        Mock<IValidator<DummyCommand>> validatorMock = new();
        IEnumerable<IValidator<DummyCommand>> validators = new List<IValidator<DummyCommand>>
        {
            validatorMock.Object
        };
        ValidationBehavior<DummyCommand, Result> validationBehavior = new(validators);
        
        DummyCommand dummyCommand = new();
        validatorMock
            .Setup(x => x.ValidateAsync(dummyCommand, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
                
        Mock<RequestHandlerDelegate<Result>> requestHandlerDelegateMock = new();
        Result result = Result.Success();
        requestHandlerDelegateMock
            .Setup(x => x())
            .ReturnsAsync(result);
        
        Result resultFromPipeline = await validationBehavior.Handle(dummyCommand, requestHandlerDelegateMock.Object, CancellationToken.None);

        Assert.That(resultFromPipeline, Is.EqualTo(result));
        
        validatorMock.Verify(x => x.ValidateAsync(dummyCommand, CancellationToken.None));
        requestHandlerDelegateMock.Verify(x => x(), Times.Once);
        
        validatorMock.VerifyNoOtherCalls();
        requestHandlerDelegateMock.VerifyNoOtherCalls();
    }
}