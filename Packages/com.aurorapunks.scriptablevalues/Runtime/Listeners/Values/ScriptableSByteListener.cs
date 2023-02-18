using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     A <see cref="ScriptableValueListener{TValue}" /> that listens to a <see cref="ScriptableValue{TValue}" /> with a
	///     type of <see cref="sbyte" /> and invokes an <see cref="UnityEngine.Events.UnityEvent" /> when the value changes.
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable SByte Listener", 1001)]
#endif
	public sealed class ScriptableSByteListener : ScriptableValueListener<sbyte> { }
}
