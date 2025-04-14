#if UNITY_2022_3_OR_NEWER // Only 2022.3 and newer versions support incremental generators.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.ScriptableValues
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	[ExcludeFromCodeCoverage]
	public sealed class GenerateScriptableCallbacksAttribute : Attribute { }
}
#endif