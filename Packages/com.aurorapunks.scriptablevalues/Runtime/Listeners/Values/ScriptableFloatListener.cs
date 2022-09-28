using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable Float Listener", 1008)]
#endif
	public sealed class ScriptableFloatListener : ScriptableValueListener<float> { }
}