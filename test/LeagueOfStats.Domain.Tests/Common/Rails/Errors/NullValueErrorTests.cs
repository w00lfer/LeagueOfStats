using LeagueOfStats.Domain.Common.Rails.Errors;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Common.Rails.Errors;

[TestFixture]
public class NullValueErrorTests
{
    [Test]
    public void Constructor_AllValid_CreatesNullValueErrorWithMessage()
    {
        NullValueError nullValueError = new NullValueError();
        
        Assert.That(nullValueError.ErrorMessage, Is.EqualTo("Null value provided"));
    }
}