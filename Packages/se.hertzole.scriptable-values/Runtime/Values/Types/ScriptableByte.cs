using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="byte"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Byte", menuName = "Hertzole/Scriptable Values/Values/Byte Value", order = ORDER + 0)]
#endif
	public sealed class ScriptableByte : ScriptableValue<byte> { }
}
