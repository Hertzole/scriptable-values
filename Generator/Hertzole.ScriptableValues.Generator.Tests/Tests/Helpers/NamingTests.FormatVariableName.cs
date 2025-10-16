using System;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Generator.Tests;

internal partial class NamingTests
{
    [Test]
    public void FormatVariableName_RemovesUnderscorePrefix()
    {
        // Arrange
        ReadOnlySpan<char> input = "_myVariable".AsSpan();
        ReadOnlySpan<char> expected = "MyVariable".AsSpan();

        // Act
        ReadOnlySpan<char> result = Naming.FormatVariableName(input);

        // Assert
        Assert.That(result.ToString(), Is.EqualTo(expected.ToString()));
    }

    [Test]
    public void FormatVariableName_RemovesDoubleUnderscorePrefix()
    {
        // Arrange
        ReadOnlySpan<char> input = "__myVariable".AsSpan();
        ReadOnlySpan<char> expected = "MyVariable".AsSpan();

        // Act
        ReadOnlySpan<char> result = Naming.FormatVariableName(input);

        // Assert
        Assert.That(result.ToString(), Is.EqualTo(expected.ToString()));
    }

    [Test]
    public void FormatVariableName_RemovesPrefix()
    {
        // Arrange
        ReadOnlySpan<char> input = "m_myVariable".AsSpan();
        ReadOnlySpan<char> expected = "MyVariable".AsSpan();

        // Act
        ReadOnlySpan<char> result = Naming.FormatVariableName(input);

        // Assert
        Assert.That(result.ToString(), Is.EqualTo(expected.ToString()));
    }

    [Test]
    public void FormatVariableName_UppercasesFirstLetter()
    {
        // Arrange
        ReadOnlySpan<char> input = "myVariable".AsSpan();
        ReadOnlySpan<char> expected = "MyVariable".AsSpan();

        // Act
        ReadOnlySpan<char> result = Naming.FormatVariableName(input);

        // Assert
        Assert.That(result.ToString(), Is.EqualTo(expected.ToString()));
    }

    [Test]
    public void FormatVariableName_EmptyString_ReturnsEmptyString()
    {
        // Arrange
        ReadOnlySpan<char> input = "".AsSpan();
        ReadOnlySpan<char> expected = "".AsSpan();

        // Act
        ReadOnlySpan<char> result = Naming.FormatVariableName(input);

        // Assert
        Assert.That(result.ToString(), Is.EqualTo(expected.ToString()));
    }

    [Test]
    public void FormatVariableName_AlreadyUppercase_ReturnsSameString()
    {
        // Arrange
        ReadOnlySpan<char> input = "MyVariable".AsSpan();
        ReadOnlySpan<char> expected = "MyVariable".AsSpan();

        // Act
        ReadOnlySpan<char> result = Naming.FormatVariableName(input);

        // Assert
        Assert.That(result.ToString(), Is.EqualTo(expected.ToString()));
    }
}