using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable UShort", menuName = "Aurora Punks/Scriptable Values/Values/UShort Value", order = 1003)]
#endif
	public sealed class ScriptableUShort : ScriptableValue<ushort> { }
}