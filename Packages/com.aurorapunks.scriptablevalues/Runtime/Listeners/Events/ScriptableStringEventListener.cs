using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable String Event Listener", 1113)]
#endif
	public sealed class ScriptableStringEventListener : ScriptableEventListener<string> { }
}