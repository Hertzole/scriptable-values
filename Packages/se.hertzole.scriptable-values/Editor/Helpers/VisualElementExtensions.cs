using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	internal static class VisualElementExtensions
	{
		public static void SetVisibility(this VisualElement element, bool visible)
		{
			element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
		}
	}
}