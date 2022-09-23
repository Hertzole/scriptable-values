using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Bool", menuName = "Aurora Punks/Scriptable Values/Values/Bool Value", order = 1011)]
#endif
	public sealed class ScriptableBool : ScriptableValue<bool> { }
}