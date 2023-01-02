using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable UInt", menuName = "Aurora Punks/Scriptable Values/Values/UInt Value", order = ORDER + 5)]
#endif
	public sealed class ScriptableUInt : ScriptableValue<uint> { }
}