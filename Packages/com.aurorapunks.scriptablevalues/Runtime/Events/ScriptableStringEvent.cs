using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable String Event", menuName = "Aurora Punks/Scriptable Values/Events/String Event", order = 1113)]
#endif
	public sealed class ScriptableStringEvent : ScriptableEvent<string> { }
}