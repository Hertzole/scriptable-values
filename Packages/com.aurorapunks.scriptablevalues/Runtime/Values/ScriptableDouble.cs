using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Double", menuName = "Aurora Punks/Scriptable Values/Values/Double Value", order = 1009)]
#endif
	public sealed class ScriptableDouble : ScriptableValue<double> { }
}