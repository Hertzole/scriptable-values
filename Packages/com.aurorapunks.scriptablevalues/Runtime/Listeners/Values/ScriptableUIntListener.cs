using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     A <see cref="ScriptableValueListener{TValue}" /> that listens to a <see cref="ScriptableValue{TValue}" /> with a
	///     type of <see cref="uint" /> and invokes an <see cref="UnityEngine.Events.UnityEvent" /> when the value changes.
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable UInt Listener", 1005)]
#endif
	public sealed class ScriptableUIntListener : ScriptableValueListener<uint> { }
}
