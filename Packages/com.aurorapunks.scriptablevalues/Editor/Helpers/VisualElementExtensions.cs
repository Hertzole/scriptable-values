using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	internal static class VisualElementExtensions
	{
		public static void SetVisibility(this VisualElement element, bool visible)
		{
			element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
		}
	}
}