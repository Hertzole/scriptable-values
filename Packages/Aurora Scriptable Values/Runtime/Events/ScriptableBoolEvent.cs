using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Bool Event", menuName = "Aurora Punks/Scriptable Values/Events/Bool Event")]
#endif
	public sealed class ScriptableBoolEvent : ScriptableEvent<bool> { }
}