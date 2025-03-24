using Xunit;

namespace Hertzole.ScriptableValues.Generator.Tests;

partial class ValueCallbackGeneratorTests
{
	private const string DEFAULT = /*lang=cs*/@"using System;
using Hertzole.ScriptableValues;

[GenerateScriptableCallbacks]
public partial class ChangedClass
{
	[GenerateValueCallback]
	public ScriptableBool valueField;
	[GenerateValueCallback]
	public ScriptableString ValueProperty { get; set; }

	private partial void OnValueFieldChanged(bool oldValue, bool newValue)
	{
		throw new System.NotImplementedException();
	}

	private partial void OnValuePropertyChanged(string oldValue, string newValue)
	{
		throw new System.NotImplementedException();
	}
}";

	[Fact]
	public void Default()
	{
		// Default is the same as Changed
		GeneratorTest.RunTest<ValueCallbackGenerator>("ChangedClass.g.cs", DEFAULT, CHANGED_EXPECTED);
	}
}