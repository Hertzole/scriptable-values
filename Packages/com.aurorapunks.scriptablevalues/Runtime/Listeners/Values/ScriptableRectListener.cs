using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     A <see cref="ScriptableValueListener{TValue}" /> that listens to a <see cref="ScriptableValue{TValue}" /> with a
	///     type of <see cref="Rect" /> and invokes an <see cref="UnityEngine.Events.UnityEvent" /> when the value changes.
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable Rect Listener", 1022)]
#endif
	public sealed class ScriptableRectListener : ScriptableValueListener<Rect> { }
}
