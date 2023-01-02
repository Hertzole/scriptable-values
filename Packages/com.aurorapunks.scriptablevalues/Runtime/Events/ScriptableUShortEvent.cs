using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable UShort Event", menuName = "Aurora Punks/Scriptable Values/Events/UShort Event", order = ORDER + 4)]
#endif
	public sealed class ScriptableUShortEvent : ScriptableEvent<ushort> { }
}