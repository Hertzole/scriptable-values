using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable Short Event Listener", 1103)]
#endif
	public sealed class ScriptableShortEventListener : ScriptableEventListener<short> { }
}