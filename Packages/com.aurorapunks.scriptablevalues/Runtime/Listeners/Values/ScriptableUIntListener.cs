using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable UInt Listener", 1005)]
#endif
	public sealed class ScriptableUIntListener : ScriptableValueListener<uint> { }
}