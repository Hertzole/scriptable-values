using UnityEditor.UIElements;
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

		public static void RegisterValueChangeCallback<TUserArgs>(this PropertyField element,
			EventCallback<SerializedPropertyChangeEvent, TUserArgs> callback,
			TUserArgs args)
		{
			element.RegisterCallback(callback, args);
		}

		public static bool RegisterValueChangedCallback<T, TUserArgs>(this INotifyValueChanged<T> control,
			EventCallback<ChangeEvent<T>, TUserArgs> callback,
			TUserArgs args)
		{
			if (control is not CallbackEventHandler handler)
			{
				return false;
			}

			handler.RegisterCallback(callback, args);
			return true;
		}
	}
}