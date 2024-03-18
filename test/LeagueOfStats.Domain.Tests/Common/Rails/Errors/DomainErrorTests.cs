using LeagueOfStats.Domain.Common.Rails.Errors;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Common.Rails.Errors;

public class DomainErrorTests
{
    [Test]
    public void Constructor_AllValid_CreatesDomainErrorWithMessage()
    {
        const string message = "error message";
        DomainError domainError = new DomainError(message);
        
        Assert.That(domainError.ErrorMessage, Is.EqualTo(message));
    }
}