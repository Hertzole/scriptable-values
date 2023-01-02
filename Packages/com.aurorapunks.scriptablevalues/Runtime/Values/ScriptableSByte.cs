using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable SByte", menuName = "Aurora Punks/Scriptable Values/Values/SByte Value", order = ORDER + 1)]
#endif
	public sealed class ScriptableSByte : ScriptableValue<sbyte> { }
}