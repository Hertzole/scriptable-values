using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Byte", menuName = "Aurora Punks/Scriptable Values/Values/Byte Value", order = 1000)]
#endif
	public sealed class ScriptableByte : ScriptableValue<byte> { }
}