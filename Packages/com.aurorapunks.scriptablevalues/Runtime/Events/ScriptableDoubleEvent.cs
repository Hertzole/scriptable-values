using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Double Event", menuName = "Aurora Punks/Scriptable Values/Events/Double Event", order = 1110)]
#endif
	public sealed class ScriptableDoubleEvent : ScriptableEvent<double> { }
}