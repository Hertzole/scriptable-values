using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Decimal Event", menuName = "Aurora Punks/Scriptable Values/Events/Decimal Event", order = 1111)]
#endif
	public sealed class ScriptableDecimalEvent : ScriptableEvent<decimal> { }
}