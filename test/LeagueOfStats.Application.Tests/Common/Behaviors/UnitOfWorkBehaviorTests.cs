using System.Data;
using LeagueOfStats.Application.Common.Behaviors;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.Tests.Common.TestCommons;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Common.Repositories;
using MediatR;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Application.Tests.Common.Behaviors;

[TestFixture]
public class UnitOfWorkBehaviorTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock.Reset();
    }

    [Test]
    public async Task Handle_QueryHasFailure_RollbackTransactionAndReturnsQueryFailure()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);
        
        UnitOfWorkBehavior<DummyQuery, Result<DummyValue>> unitOfWorkBehavior = new(_unitOfWorkMock.Object);
        
        Mock<RequestHandlerDelegate<Result<DummyValue>>> requestHandlerDelegateMock = new();
        Result<DummyValue> result = Result.Failure<DummyValue>(new ApplicationError("error"));
        requestHandlerDelegateMock
            .Setup(x => x())
            .ReturnsAsync(result);
        
        DummyQuery dummyQuery = new();
        Result<DummyValue> resultFromPipeline = await unitOfWorkBehavior.Handle(dummyQuery, requestHandlerDelegateMock.Object, CancellationToken.None);
        
        Assert.That(resultFromPipeline, Is.EqualTo(result));
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        requestHandlerDelegateMock.Verify(x => x(), Times.Once);
        dbTransactionMock.Verify(x => x.Rollback(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        
        _unitOfWorkMock.VerifyNoOtherCalls();
        requestHandlerDelegateMock.VerifyNoOtherCalls();
        dbTransactionMock.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Handle_QueryHasSuccess_SaveChangesAndCommitsTransactionAndReturnsQuerySuccess()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);
        
        UnitOfWorkBehavior<DummyQuery, Result<DummyValue>> unitOfWorkBehavior = new(_unitOfWorkMock.Object);
        
        Mock<RequestHandlerDelegate<Result<DummyValue>>> requestHandlerDelegateMock = new();
        Result<DummyValue> result = Result.Success(new DummyValue());
        requestHandlerDelegateMock
            .Setup(x => x())
            .ReturnsAsync(result);
        
        DummyQuery dummyQuery = new();
        Result<DummyValue> resultFromPipeline = await unitOfWorkBehavior.Handle(dummyQuery, requestHandlerDelegateMock.Object, CancellationToken.None);
        
        Assert.That(resultFromPipeline, Is.EqualTo(result));
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        requestHandlerDelegateMock.Verify(x => x(), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        dbTransactionMock.Verify(x => x.Commit(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        
        _unitOfWorkMock.VerifyNoOtherCalls();
        requestHandlerDelegateMock.VerifyNoOtherCalls();
        dbTransactionMock.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Handle_CommandHasFailure_RollbackTransactionAndReturnsCommandFailure()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);
        
        UnitOfWorkBehavior<DummyQuery, Result> unitOfWorkBehavior = new(_unitOfWorkMock.Object);
        
        Mock<RequestHandlerDelegate<Result>> requestHandlerDelegateMock = new();
        Result result = Result.Failure(new ApplicationError("error"));
        requestHandlerDelegateMock
            .Setup(x => x())
            .ReturnsAsync(result);
        
        DummyQuery dummyQuery = new();
        Result resultFromPipeline = await unitOfWorkBehavior.Handle(dummyQuery, requestHandlerDelegateMock.Object, CancellationToken.None);
        
        Assert.That(resultFromPipeline, Is.EqualTo(result));
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        requestHandlerDelegateMock.Verify(x => x(), Times.Once);
        dbTransactionMock.Verify(x => x.Rollback(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        
        _unitOfWorkMock.VerifyNoOtherCalls();
        requestHandlerDelegateMock.VerifyNoOtherCalls();
        dbTransactionMock.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Handle_CommandHasSuccess_SaveChangesAndCommitsTransactionAndReturnsCommandSuccess()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);
        
        UnitOfWorkBehavior<DummyCommand, Result> unitOfWorkBehavior = new(_unitOfWorkMock.Object);
        
        Mock<RequestHandlerDelegate<Result>> requestHandlerDelegateMock = new();
        Result result = Result.Success();
        requestHandlerDelegateMock
            .Setup(x => x())
            .ReturnsAsync(result);
        
        DummyCommand dummyCommand = new();
        Result resultFromPipeline = await unitOfWorkBehavior.Handle(dummyCommand, requestHandlerDelegateMock.Object, CancellationToken.None);
        
        Assert.That(resultFromPipeline, Is.EqualTo(result));
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        requestHandlerDelegateMock.Verify(x => x(), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        dbTransactionMock.Verify(x => x.Commit(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        
        _unitOfWorkMock.VerifyNoOtherCalls();
        requestHandlerDelegateMock.VerifyNoOtherCalls();
        dbTransactionMock.VerifyNoOtherCalls();
    }
}