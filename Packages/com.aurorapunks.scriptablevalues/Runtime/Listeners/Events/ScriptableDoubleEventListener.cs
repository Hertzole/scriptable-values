using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable Double Event Listener", 1110)]
#endif
	public sealed class ScriptableDoubleEventListener : ScriptableEventListener<double> { }
}