using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ScriptableEventListener{TValue}" /> that listens to a <see cref="ScriptableEvent{TValue}" /> with a
	///     type of <see cref="BoundsInt" /> and invokes an <see cref="UnityEngine.Events.UnityEvent" /> when the event is invoked.
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable BoundsInt Event Listener", 1125)]
#endif
	public sealed class ScriptableBoundsIntEventListener : ScriptableEventListener<BoundsInt> { }
}
