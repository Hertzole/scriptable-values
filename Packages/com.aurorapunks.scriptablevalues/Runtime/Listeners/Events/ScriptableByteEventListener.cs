using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable Byte Event Listener", 1101)]
#endif
	public sealed class ScriptableByteEventListener : ScriptableEventListener<byte> { }
}