using LeagueOfStats.API.Common.Errors;
using NUnit.Framework;

namespace LeagueOfStats.API.Tests.Common.Errors;

[TestFixture]
public class ApiErrorTests
{
    [Test]
    public void Constructor_AllValid_CreatesDomainErrorWithMessage()
    {
        const string message = "error message";
        ApiError apiError = new ApiError(message);
        
        Assert.That(apiError.ErrorMessage, Is.EqualTo(message));
    }
}