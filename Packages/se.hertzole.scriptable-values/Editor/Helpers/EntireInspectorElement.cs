#nullable enable

using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	public sealed class EntireInspectorElement : VisualElement
	{
		private IMGUIContainer? imguiContainer;
		private VisualElement? contentViewport;

		public EntireInspectorElement()
		{
			style.flexGrow = 1;
			style.minHeight = 440;

			RegisterCallback<GeometryChangedEvent, EntireInspectorElement>((_, element) =>
			{
				// Only do this if the view port is null to save resources.
				if (element.contentViewport == null)
				{
					VisualElement? editorElement = FindParent(element, "EditorElement");
					if (editorElement != null)
					{
						element.imguiContainer = editorElement.Q<IMGUIContainer>();
						if (element.imguiContainer != null)
						{
							element.contentViewport = FindParent<VisualElement>(element, "unity-content-viewport");
							Assert.IsNotNull(element.contentViewport);

							element.contentViewport!.RegisterCallback<GeometryChangedEvent, EntireInspectorElement>((_, args) => { args.UpdateHeight(); },
								element);

							UpdateHeight();
						}
					}
				}
			}, this);
		}

		private void UpdateHeight()
		{
			if (contentViewport != null && imguiContainer != null)
			{
				style.height = contentViewport.resolvedStyle.height - imguiContainer.resolvedStyle.height - 16;
			}
		}

		private static VisualElement? FindParent(VisualElement element, string typeName)
		{
			VisualElement parent = element;
			do
			{
				parent = parent.parent;
				if (parent != null && parent.GetType().Name == typeName)
				{
					return parent;
				}
			} while (parent != null);

			return null;
		}

		private static T? FindParent<T>(VisualElement element, string? name = null) where T : VisualElement
		{
			VisualElement parent = element;
			do
			{
				parent = parent.hierarchy.parent;
				if (parent != null && parent.GetType() == typeof(T))
				{
					if (!string.IsNullOrEmpty(name) && parent.name != name)
					{
						continue;
					}

					return (T) parent;
				}
			} while (parent != null);

			return null;
		}
	}
}