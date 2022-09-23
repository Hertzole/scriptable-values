using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Float Event", menuName = "Aurora Punks/Scriptable Values/Events/Float Event", order = 1109)]
#endif
	public sealed class ScriptableFloatEvent : ScriptableEvent<float> { }
}