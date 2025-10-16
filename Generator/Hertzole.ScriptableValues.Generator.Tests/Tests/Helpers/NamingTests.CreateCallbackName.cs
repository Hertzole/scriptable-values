using NUnit.Framework;

namespace Hertzole.ScriptableValues.Generator.Tests;

public partial class NamingTests
{
    [Test]
    [TestCase("myValue", "OnMyValueChanged")]
    public void CreateCallbackName_ValueChanged(string name, string expected)
    {
        // Act
        string result = Naming.CreateCallbackName(name, ScriptableType.Value, CallbackFlags.PostInvoke);
        
        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}