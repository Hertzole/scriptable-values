using System;
using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Char"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Char", menuName = "Aurora Punks/Scriptable Values/Values/Char Value", order = ORDER + 13)]
#endif
	public sealed class ScriptableChar : ScriptableValue<Char> { }
}
