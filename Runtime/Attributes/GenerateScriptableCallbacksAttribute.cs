#if UNITY_2022_3_OR_NEWER // Only 2022.3 and newer versions support incremental generators.
using System;

namespace Hertzole.ScriptableValues
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public sealed class GenerateScriptableCallbacksAttribute : Attribute { }
}
#endif