using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable Double Listener", 1009)]
#endif
	public sealed class ScriptableDoubleListener : ScriptableValueListener<double> { }
}