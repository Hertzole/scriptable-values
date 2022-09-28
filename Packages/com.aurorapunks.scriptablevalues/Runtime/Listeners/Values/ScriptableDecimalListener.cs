using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable Decimal Listener", 1010)]
#endif
	public sealed class ScriptableDecimalListener : ScriptableValueListener<decimal> { }
}