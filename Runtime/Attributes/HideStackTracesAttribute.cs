using System;
using System.Diagnostics;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     Use on your own <see cref="RuntimeScriptableObject" /> derived classes to hide the stack traces in the inspector.
	/// </summary>
	[Conditional("UNITY_EDITOR")]
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class HideStackTracesAttribute : Attribute { }
}