using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="string"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable String", menuName = "Hertzole/Scriptable Values/Values/String Value", order = ORDER + 12)]
#endif
	public sealed class ScriptableString : ScriptableValue<string> { }
}
