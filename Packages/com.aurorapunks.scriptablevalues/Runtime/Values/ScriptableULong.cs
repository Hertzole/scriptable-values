using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable ULong", menuName = "Aurora Punks/Scriptable Values/Values/ULong Value", order = 1007)]
#endif
	public sealed class ScriptableULong : ScriptableValue<ulong> { }
}