using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Long", menuName = "Aurora Punks/Scriptable Values/Values/Long Value", order = 1006)]
#endif
	public sealed class ScriptableLong : ScriptableValue<long> { }
}