using LeagueOfStats.Domain.Common.Rails.Errors;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Common.Errors;

[TestFixture]
public class EntityNotFoundErrorTests
{
    [Test]
    public void Constructor_AllValid_CreateEntityNotFoundErrorWithMessage()
    {
        const string message = "error message";
        EntityNotFoundError entityNotFoundError = new EntityNotFoundError(message);
        
        Assert.That(entityNotFoundError.ErrorMessage, Is.EqualTo(message));
    }
}