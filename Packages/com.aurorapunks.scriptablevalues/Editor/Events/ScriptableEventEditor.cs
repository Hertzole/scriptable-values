using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableEvent), true)]
	public class ScriptableEventEditor : UnityEditor.Editor
	{
		private SerializedProperty onInvoked;

		private ScriptableEvent scriptableEvent;

		private StackTraceElement<ScriptableEvent> stackTraceElement;
		private VisualElement contentViewport;

		protected virtual void OnEnable()
		{
			scriptableEvent = (ScriptableEvent) target;

			onInvoked = serializedObject.FindProperty(nameof(onInvoked));
		}

		protected virtual void OnDisable()
		{
			stackTraceElement?.Dispose();
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new EntireInspectorElement();

			PropertyField onInvokedField = new PropertyField(onInvoked);
			onInvokedField.Bind(serializedObject);

			root.Add(onInvokedField);

			stackTraceElement = new StackTraceElement<ScriptableEvent>(scriptableEvent)
			{
				style =
				{
					marginTop = 4
				}
			};
			root.Add(stackTraceElement);

			return root;
		}
	}
}