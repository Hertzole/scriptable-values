using System;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Generator.Tests;

internal partial class NamingTests
{
    [Test]
    // Values
    [TestCase("myValue", "OnMyValueChanged", ScriptableType.Value, CallbackFlags.PostInvoke)]
    [TestCase("myValue", "OnMyValueChanging", ScriptableType.Value, CallbackFlags.PreInvoke)]
    [TestCase("m_myValue", "OnMyValueChanged", ScriptableType.Value, CallbackFlags.PostInvoke)]
    [TestCase("m_myValue", "OnMyValueChanging", ScriptableType.Value, CallbackFlags.PreInvoke)]
    [TestCase("onMyValue", "OnMyValueChanged", ScriptableType.Value, CallbackFlags.PostInvoke)]
    [TestCase("onMyValue", "OnMyValueChanging", ScriptableType.Value, CallbackFlags.PreInvoke)]
    [TestCase("onlyValue", "OnOnlyValueChanged", ScriptableType.Value, CallbackFlags.PostInvoke)]
    [TestCase("onlyValue", "OnOnlyValueChanging", ScriptableType.Value, CallbackFlags.PreInvoke)]
    // Events
    [TestCase("myValue", "OnMyValueInvoked", ScriptableType.Event)]
    [TestCase("myValue", "OnMyValueInvoked", ScriptableType.GenericEvent)]
    [TestCase("m_myValue", "OnMyValueInvoked", ScriptableType.Event)]
    [TestCase("m_myValue", "OnMyValueInvoked", ScriptableType.GenericEvent)]
    [TestCase("onMyValue", "OnMyValueInvoked", ScriptableType.Event)]
    [TestCase("onMyValue", "OnMyValueInvoked", ScriptableType.GenericEvent)]
    [TestCase("onlyValue", "OnOnlyValueInvoked", ScriptableType.Event)]
    [TestCase("onlyValue", "OnOnlyValueInvoked", ScriptableType.GenericEvent)]
    // Pool, list, and dictionary
    [TestCase("myValue", "OnMyValueChanged", ScriptableType.Pool)]
    [TestCase("myValue", "OnMyValueChanged", ScriptableType.Pool)]
    [TestCase("m_myValue", "OnMyValueChanged", ScriptableType.List)]
    [TestCase("m_myValue", "OnMyValueChanged", ScriptableType.List)]
    [TestCase("onMyValue", "OnMyValueChanged", ScriptableType.Dictionary)]
    [TestCase("onMyValue", "OnMyValueChanged", ScriptableType.Dictionary)]
    [TestCase("onlyValue", "OnOnlyValueChanged", ScriptableType.Pool)]
    [TestCase("onlyValue", "OnOnlyValueChanged", ScriptableType.List)]
    public void CreateCallbackName(string name, string expected, ScriptableType type, CallbackFlags flags = CallbackFlags.None)
    {
        // Act
        string result = Naming.CreateCallbackName(name, type, flags);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CreateCallbackName_InvalidType_ThrowsException()
    {
        // Arrange
        const string name = "myValue";
        const ScriptableType invalid_type = (ScriptableType) 999;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Naming.CreateCallbackName(name, invalid_type, CallbackFlags.None));
    }
}