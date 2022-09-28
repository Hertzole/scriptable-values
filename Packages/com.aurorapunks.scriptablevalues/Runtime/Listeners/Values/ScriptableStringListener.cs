using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Values/Scriptable String Listener", 1012)]
#endif
	public sealed class ScriptableStringListener : ScriptableValueListener<string> { }
}