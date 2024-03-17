using LeagueOfStats.Domain.Common.Rails.Errors;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Common.Errors;

public class DomainErrorTests
{
    [Test]
    public void Constructor_AllValid_CreateDomainErrorWithMessage()
    {
        const string message = "error message";
        DomainError domainError = new DomainError(message);
        
        Assert.That(domainError.ErrorMessage, Is.EqualTo(message));
    }
}