using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Byte", menuName = "Aurora Punks/Scriptable Values/Values/Byte Value", order = ORDER)]
#endif
	public sealed class ScriptableByte : ScriptableValue<byte> { }
}