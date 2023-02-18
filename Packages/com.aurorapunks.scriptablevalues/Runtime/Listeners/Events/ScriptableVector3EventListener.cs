using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     A <see cref="ScriptableEventListener{TValue}" /> that listens to a <see cref="ScriptableEvent{TValue}" /> with a
	///     type of <see cref="Vector3" /> and invokes an <see cref="UnityEngine.Events.UnityEvent" /> when the event is invoked.
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable Vector3 Listener", 1117)]
#endif
	public sealed class ScriptableVector3EventListener : ScriptableEventListener<Vector3> { }
}
