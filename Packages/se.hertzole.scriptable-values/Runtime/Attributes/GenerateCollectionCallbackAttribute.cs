#if UNITY_2022_3_OR_NEWER // Only 2022.3 and newer versions support incremental generators.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     Mark a field or property to generate a collection callback. The target must derive from
	///     <see cref="ScriptableList{T}" /> or <see cref="ScriptableDictionary{TKey,TValue}" />.
	/// </summary>
	/// <remarks>
	///     The containing type must be marked with <see cref="GenerateScriptableCallbacksAttribute" /> in order to
	///     generate any callbacks.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ExcludeFromCodeCoverage]
	public sealed class GenerateCollectionCallbackAttribute : Attribute { }
}
#endif