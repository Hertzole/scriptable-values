using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Decimal", menuName = "Aurora Punks/Scriptable Values/Values/Decimal Value", order = ORDER + 10)]
#endif
	public sealed class ScriptableDecimal : ScriptableValue<decimal> { }
}