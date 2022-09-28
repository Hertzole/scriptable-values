using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable SByte Listener", 1001)]
#endif
	public sealed class ScriptableSByteListener : ScriptableValueListener<sbyte> { }
}