using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable Bool Listener", 1011)]
#endif
	public sealed class BoolValueListener : ValueListener<bool> { }
}