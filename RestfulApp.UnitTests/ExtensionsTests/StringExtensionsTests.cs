using FluentAssertions;
using RestfulApp.Api.Extensions;
using Xunit;

namespace RestfulApp.UnitTests.ExtensionsTests;
public class StringExtensionsTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ToLowerCamelCase_ReturnsEmptyString_WhenStringIsNullOrEmpty(string value)
    {
        value.ToLowerCamelCase().Should().BeEmpty();
    }

    [Theory]
    [InlineData("A")]
    [InlineData("String")]
    public void ToLowerCamelCase_ReturnsStringWithFirstCharInLowerCase(string value)
    {
        value.ToLowerCamelCase()[0].ToString().Should().BeLowerCased();
    }
}