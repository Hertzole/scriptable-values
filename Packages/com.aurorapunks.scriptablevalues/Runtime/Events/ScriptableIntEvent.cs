using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Event", menuName = "Aurora Punks/Scriptable Values/Events/Int Event")]
#endif
	public sealed class ScriptableIntEvent : ScriptableEvent<int> { }
}