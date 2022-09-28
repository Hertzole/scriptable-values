using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable UShort Listener", 1003)]
#endif
	public sealed class ScriptableUShortListener : ScriptableValueListener<ushort> { }
}