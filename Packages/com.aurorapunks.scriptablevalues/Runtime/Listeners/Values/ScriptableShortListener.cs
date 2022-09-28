using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable Short Listener", 1002)]
#endif
	public sealed class ScriptableShortListener : ScriptableValueListener<short> { }
}