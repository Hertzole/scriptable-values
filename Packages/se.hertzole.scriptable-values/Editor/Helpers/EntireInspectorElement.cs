using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	public sealed class EntireInspectorElement : VisualElement
	{
		private IMGUIContainer imguiContainer;
		private VisualElement contentViewport;

		public EntireInspectorElement()
		{
			style.flexGrow = 1;

			RegisterCallback<GeometryChangedEvent, VisualElement>((_, root) =>
			{
				// Only do this if the view port is null to save resources.
				if (contentViewport == null)
				{
					VisualElement editorElement = FindParent(root, "EditorElement");
					if (editorElement != null)
					{
						imguiContainer = editorElement.Q<IMGUIContainer>();
						if (imguiContainer != null)
						{
							contentViewport = FindParent<TemplateContainer>(root);
						}
					}
				}

				// The viewport exists.
				if (contentViewport != null && imguiContainer != null)
				{
					// Update the root size to match the entire inspector.
					root.style.height = contentViewport.resolvedStyle.height - imguiContainer.resolvedStyle.height - 32;
				}
			}, this);
		}

		private static VisualElement FindParent(VisualElement element, string typeName)
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

		private static T FindParent<T>(VisualElement element, string name = null) where T : VisualElement
		{
			VisualElement parent = element;
			do
			{
				parent = parent.parent;
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