using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable Long Event Listener", 1107)]
#endif
	public sealed class ScriptableLongEventListener : ScriptableEventListener<long> { }
}