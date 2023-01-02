using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Double", menuName = "Aurora Punks/Scriptable Values/Values/Double Value", order = ORDER + 9)]
#endif
	public sealed class ScriptableDouble : ScriptableValue<double> { }
}