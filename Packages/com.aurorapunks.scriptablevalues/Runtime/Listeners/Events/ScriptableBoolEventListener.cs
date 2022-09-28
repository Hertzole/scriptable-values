using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable Bool Event Listener", 1112)]
#endif
	public sealed class ScriptableBoolEventListener : ScriptableEventListener<bool> { }
}