using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable Byte Listener", 1000)]
#endif
	public sealed class ByteValueListener : ValueListener<byte> { }
}