using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable String", menuName = "Aurora Punks/Scriptable Values/Values/String Value", order = ORDER + 12)]
#endif
	public sealed class ScriptableString : ScriptableValue<string> { }
}