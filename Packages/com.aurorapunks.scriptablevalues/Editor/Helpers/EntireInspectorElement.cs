using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	public sealed class EntireInspectorElement : VisualElement
	{
		private VisualElement contentViewport;
		
		public EntireInspectorElement()
		{
			RegisterCallback<GeometryChangedEvent, VisualElement>((_, root) =>
			{
				// Only do this if the view port is null to save resources.
				if (contentViewport == null)
				{
					// Find the template container.
					TemplateContainer rootVisualContainer = FindParent<TemplateContainer>(root);
					if (rootVisualContainer != null)
					{
						// Find the view port element.
						contentViewport = rootVisualContainer.Q<VisualElement>("unity-content-viewport");
					}
				}

				// The viewport exists.
				if (contentViewport != null)
				{
					// Update the root size to match the entire inspector.
					root.style.height = contentViewport.resolvedStyle.height - 70;
				}
			}, this);
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