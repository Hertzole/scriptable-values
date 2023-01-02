using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable UShort", menuName = "Aurora Punks/Scriptable Values/Values/UShort Value", order = ORDER + 3)]
#endif
	public sealed class ScriptableUShort : ScriptableValue<ushort> { }
}