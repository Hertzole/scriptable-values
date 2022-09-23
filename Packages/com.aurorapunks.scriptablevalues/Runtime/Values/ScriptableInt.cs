using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Int", menuName = "Aurora Punks/Scriptable Values/Values/Int Value", order = 1004)]
#endif
	public sealed class ScriptableInt : ScriptableValue<int> { }
}