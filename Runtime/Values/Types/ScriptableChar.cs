using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="char"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Char", menuName = "Hertzole/Scriptable Values/Values/Char Value", order = ORDER + 13)]
#endif
	public sealed class ScriptableChar : ScriptableValue<char> { }
}
