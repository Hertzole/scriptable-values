using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable ULong Event", menuName = "Aurora Punks/Scriptable Values/Events/ULong Event", order = ORDER + 8)]
#endif
	public sealed class ScriptableULongEvent : ScriptableEvent<ulong> { }
}