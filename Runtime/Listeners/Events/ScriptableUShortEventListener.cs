using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ScriptableEventListener{TValue}" /> that listens to a <see cref="ScriptableEvent{TValue}" /> with a
	///     type of <see cref="ushort" /> and invokes an <see cref="UnityEngine.Events.UnityEvent" /> when the event is invoked.
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable UShort Event Listener", 1103)]
#endif
	public sealed class ScriptableUShortEventListener : ScriptableEventListener<ushort> { }
}
