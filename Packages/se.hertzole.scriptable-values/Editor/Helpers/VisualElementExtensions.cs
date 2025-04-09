using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	internal static class VisualElementExtensions
	{
		public static VisualElement SetVisibility(this VisualElement element, bool visible)
		{
			element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
			return element;
		}

		public static VisualElement AddSpace(this VisualElement element, float height = 8)
		{
			element.style.marginTop = element.resolvedStyle.marginTop + height;
			return element;
		}
	}
}