using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable Long Listener", 1006)]
#endif
	public sealed class LongValueListener : ValueListener<long> { }
}