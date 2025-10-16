using Hertzole.ScriptableValues;
using UnityEngine;

namespace My.Namespace
{
	[CreateAssetMenu]
	[HideStackTraces]
	public class MySo : RuntimeScriptableObject
	{
		[SerializeField] 
		private ValueReference<int> constantValue = default;
		[SerializeField] 
		private ValueReference<int> referenceValue = default;
		[SerializeField] 
		private ValueReference<int> example = default;
	}
}