using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable Decimal Event Listener", 1111)]
#endif
	public sealed class ScriptableDecimalEventListener : ScriptableEventListener<decimal> { }
}