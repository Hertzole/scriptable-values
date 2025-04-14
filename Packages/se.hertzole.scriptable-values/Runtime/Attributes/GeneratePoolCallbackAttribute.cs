#if UNITY_2022_3_OR_NEWER // Only 2022.3 and newer versions support incremental generators.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.ScriptableValues
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ExcludeFromCodeCoverage]
	public sealed class GeneratePoolCallbackAttribute : Attribute { }
}
#endif