using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     A <see cref="ScriptableEventListener{TValue}" /> that listens to a <see cref="ScriptableEvent{TValue}" /> with a
	///     type of <see cref="short" /> and invokes an <see cref="UnityEngine.Events.UnityEvent" /> when the event is invoked..
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable UShort Event Listener", 1104)]
#endif
	public sealed class ScriptableUShortEventListener : ScriptableEventListener<ushort> { }
}