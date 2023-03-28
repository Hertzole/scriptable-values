using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="BoundsInt"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Bounds Int Event", menuName = "Hertzole/Scriptable Values/Events/BoundsInt Event", order = ORDER + 26)]
#endif
	public sealed class ScriptableBoundsIntEvent : ScriptableEvent<BoundsInt> { }
}
