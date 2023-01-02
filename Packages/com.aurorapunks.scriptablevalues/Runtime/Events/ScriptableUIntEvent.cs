using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable UInt Event", menuName = "Aurora Punks/Scriptable Values/Events/UInt Event", order = ORDER + 6)]
#endif
	public sealed class ScriptableUIntEvent : ScriptableEvent<uint> { }
}