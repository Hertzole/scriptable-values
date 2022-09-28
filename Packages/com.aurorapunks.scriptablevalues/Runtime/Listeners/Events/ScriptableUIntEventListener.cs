using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable UInt Event Listener", 1106)]
#endif
	public sealed class ScriptableUIntEventListener : ScriptableEventListener<uint> { }
}