using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableEvent), true)]
	public class ScriptableEventEditor : UnityEditor.Editor
	{
		private ScriptableEvent scriptableEvent;
		private SerializedProperty onInvoked;
		private SerializedProperty collectStackTraces;

		private StackTraceElement stackTraces;
		private VisualElement contentViewport;

		protected virtual void OnEnable()
		{
			scriptableEvent = (ScriptableEvent) target;

			onInvoked = serializedObject.FindProperty(nameof(onInvoked));
			collectStackTraces = serializedObject.FindProperty(nameof(collectStackTraces));
		}

		protected virtual void OnDisable()
		{
			stackTraces?.Dispose();
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new EntireInspectorElement();

			PropertyField onInvokedField = new PropertyField(onInvoked);
			onInvokedField.Bind(serializedObject);

			stackTraces = new StackTraceElement(scriptableEvent, collectStackTraces, "Invocation Stack Traces")
			{
				style =
				{
					marginTop = 4
				}
			};

			VisualElement invokeElement = CreateInvokeButton();

			if (invokeElement != null)
			{
				invokeElement.style.marginBottom = 8;
				root.Add(invokeElement);
			}

			root.Add(onInvokedField);
			root.Add(stackTraces);

			return root;
		}

		protected virtual VisualElement CreateInvokeButton()
		{
			return new Button(OnClickInvoke)
			{
				text = "Invoke"
			};
		}

		private void OnClickInvoke()
		{
			scriptableEvent.Invoke(this);
		}
	}
}