using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable UInt Event", menuName = "Aurora Punks/Scriptable Values/Events/UInt Event", order = 1106)]
#endif
	public sealed class ScriptableUIntEvent : ScriptableEvent<uint> { }
}