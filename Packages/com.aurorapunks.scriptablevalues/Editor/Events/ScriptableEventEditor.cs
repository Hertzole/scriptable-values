using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableEvent), true)]
	public class ScriptableEventEditor : UnityEditor.Editor
	{
		private ScriptableEvent scriptableEvent;

		private StackTraceElement<ScriptableEvent> stackTraceElement;

		private VisualElement contentViewport;

		private void OnEnable()
		{
			scriptableEvent = (ScriptableEvent) target;
		}

		private void OnDisable()
		{
			Debug.Log(scriptableEvent);
			stackTraceElement?.Dispose();
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new VisualElement();

			stackTraceElement = new StackTraceElement<ScriptableEvent>(scriptableEvent);
			root.Add(stackTraceElement);

			root.RegisterCallback<GeometryChangedEvent, VisualElement>((evt, rootArgs) =>
			{
				if (contentViewport == null)
				{
					TemplateContainer rootVisualContainer = FindParent<TemplateContainer>(rootArgs);
					if (rootVisualContainer != null)
					{
						contentViewport = rootVisualContainer.Q<VisualElement>("unity-content-viewport");
						rootArgs.style.height = contentViewport.resolvedStyle.height - 70;
					}
				}
				else
				{
					rootArgs.style.height = contentViewport.resolvedStyle.height - 70;
				}
			}, root);

			return root;
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