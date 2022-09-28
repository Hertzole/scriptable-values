using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable ULong Listener", 1007)]
#endif
	public sealed class ULongValueListener : ValueListener<ulong> { }
}