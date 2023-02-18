using System;
using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="Char"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Char Event", menuName = "Aurora Punks/Scriptable Values/Events/Char Event", order = ORDER + 14)]
#endif
	public sealed class ScriptableCharEvent : ScriptableEvent<Char> { }
}
