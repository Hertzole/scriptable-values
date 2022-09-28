using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable Int Event Listener", 1105)]
#endif
	public sealed class ScriptableIntEventListener : ScriptableEventListener<int> { }
}