#if UNITY_2022_3_OR_NEWER // Only 2022.3 and newer versions support incremental generators.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     Mark a field or property to generate an event callback. The target must derive from <see cref="ScriptableEvent" />
	///     or <see cref="ScriptableEvent{T}" />.
	/// </summary>
	/// <remarks>
	///     The containing type must be marked with <see cref="GenerateScriptableCallbacksAttribute" /> in order to
	///     generate any callbacks.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ExcludeFromCodeCoverage]
	public sealed class GenerateEventCallbackAttribute : Attribute { }
}
#endif