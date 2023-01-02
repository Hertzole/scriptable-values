using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Long Event", menuName = "Aurora Punks/Scriptable Values/Events/Long Event", order = ORDER + 7)]
#endif
	public sealed class ScriptableLongEvent : ScriptableEvent<long> { }
}