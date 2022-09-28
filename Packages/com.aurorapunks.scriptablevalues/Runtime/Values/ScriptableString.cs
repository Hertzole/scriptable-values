using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable String", menuName = "Aurora Punks/Scriptable Values/Values/String Value", order = 1012)]
#endif
	public sealed class ScriptableString : ScriptableValue<string> { }
}