using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ScriptableValueListener{TValue}" /> that listens to a <see cref="ScriptableValue{TValue}" /> with a
	///     type of <see cref="BoundsInt" /> and invokes an <see cref="UnityEngine.Events.UnityEvent" /> when the value changes.
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable BoundsInt Listener", 1025)]
#endif
	public sealed class ScriptableBoundsIntListener : ScriptableValueListener<BoundsInt> { }
}
