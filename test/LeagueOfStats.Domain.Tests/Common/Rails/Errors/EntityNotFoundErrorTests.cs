using LeagueOfStats.Domain.Common.Rails.Errors;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Common.Rails.Errors;

[TestFixture]
public class EntityNotFoundErrorTests
{
    [Test]
    public void Constructor_AllValid_CreatesEntityNotFoundErrorWithMessage()
    {
        const string message = "error message";
        EntityNotFoundError entityNotFoundError = new EntityNotFoundError(message);
        
        Assert.That(entityNotFoundError.ErrorMessage, Is.EqualTo(message));
    }
}