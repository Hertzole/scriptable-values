using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Byte Event", menuName = "Aurora Punks/Scriptable Values/Events/Byte Event", order = 1101)]
#endif
	public sealed class ScriptableByteEvent : ScriptableEvent<byte> { }
}