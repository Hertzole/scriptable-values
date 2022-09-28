using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable Int Listener", 1004)]
#endif
	public sealed class IntValueListener : ValueListener<int> { }
}