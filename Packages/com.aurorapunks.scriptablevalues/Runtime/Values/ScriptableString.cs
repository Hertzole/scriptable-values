using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a string value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable String", menuName = "Aurora Punks/Scriptable Values/Values/String Value", order = ORDER + 12)]
#endif
	public sealed class ScriptableString : ScriptableValue<string> { }
}