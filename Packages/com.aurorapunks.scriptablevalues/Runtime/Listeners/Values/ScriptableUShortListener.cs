using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     A <see cref="ScriptableValueListener{TValue}" /> that listens to a <see cref="ScriptableValue{TValue}" /> with a
	///     type of <see cref="ushort" /> and invokes an <see cref="UnityEngine.Events.UnityEvent" /> when the value changes.
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable UShort Listener", 1003)]
#endif
	public sealed class ScriptableUShortListener : ScriptableValueListener<ushort> { }
}