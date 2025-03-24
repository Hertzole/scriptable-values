using System;

namespace Hertzole.ScriptableValues
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public sealed class GenerateScriptableCallbacksAttribute : Attribute { }
}