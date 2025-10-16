using NUnit.Framework;

namespace Hertzole.ScriptableValues.Generator.Tests;

partial class GeneratorTests
{
	[Test]
	public void NoMarkerAttribute_DoesNotGenerate()
	{
		string source = /*lang=cs*/@"using System;
using Hertzole.ScriptableValues;

public partial class MyClass
{
	[GenerateValueCallback]
	public ScriptableBool value;
}";

		AssertDoesNotGenerate<ScriptableCallbackGenerator>(source);
	}

	[Test]
	public void ReadOnlyStruct_DoesNotGenerate()
	{
		string source = /*lang=cs*/@"using System;
using Hertzole.ScriptableValues;

[GenerateScriptableCallbacks]
public partial readonly struct MyStruct
{
	[GenerateValueCallback]
 	public ScriptableBool value;
}";

		AssertDoesNotGenerate<ScriptableCallbackGenerator>(source);
	}

	[Test]
	public void StaticClass_DoesNotGenerate()
	{
		string source = /*lang=cs*/@"using System;
using Hertzole.ScriptableValues;

[GenerateScriptableCallbacks]
public static partial class MyClass
{
	[GenerateValueCallback]
	public ScriptableBool value;
}";

		AssertDoesNotGenerate<ScriptableCallbackGenerator>(source);
	}
}