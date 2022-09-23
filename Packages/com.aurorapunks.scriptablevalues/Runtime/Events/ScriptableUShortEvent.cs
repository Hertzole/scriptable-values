using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable UShort Event", menuName = "Aurora Punks/Scriptable Values/Events/UShort Event", order = 1104)]
#endif
	public sealed class ScriptableUShortEvent : ScriptableEvent<ushort> { }
}