#if UNITY_2022_3_OR_NEWER
using Hertzole.ScriptableValues;

namespace My.Namespace
{
	[GenerateScriptableCallbacks]
	public partial class ChangingClass
	{
		[GenerateValueCallback]
		public ScriptableString ValueProperty { get; set; }

		private partial void OnValuePropertyChanged(string oldValue, string newValue) { }
	}
}
#endif