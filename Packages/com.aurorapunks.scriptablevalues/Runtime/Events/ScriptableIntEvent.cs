using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Int Event", menuName = "Aurora Punks/Scriptable Values/Events/Int Event", order = ORDER + 5)]
#endif
	public sealed class ScriptableIntEvent : ScriptableEvent<int> { }
}