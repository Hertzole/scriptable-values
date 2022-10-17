using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableEventListener))]
	public class ScriptableEventListenerEditor : UnityEditor.Editor
	{
		private PropertyField targetEventField;
		private PropertyField startListeningField;
		private PropertyField stopListeningField;
		private PropertyField onInvokedField;

		private SerializedProperty targetEvent;
		private SerializedProperty startListening;
		private SerializedProperty stopListening;
		private SerializedProperty onInvoked;

		private readonly VisualElement[] spaces = new VisualElement[SPACES_COUNT];

		private const int SPACES_COUNT = 2;

		protected virtual void OnEnable()
		{
			targetEvent = serializedObject.FindProperty(nameof(targetEvent));
			startListening = serializedObject.FindProperty(nameof(startListening));
			stopListening = serializedObject.FindProperty(nameof(stopListening));
			onInvoked = serializedObject.FindProperty(nameof(onInvoked));
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new VisualElement();

			targetEventField = new PropertyField(targetEvent);
			startListeningField = new PropertyField(startListening);
			stopListeningField = new PropertyField(stopListening);
			onInvokedField = new PropertyField(onInvoked);

			targetEventField.Bind(serializedObject);
			startListeningField.Bind(serializedObject);
			stopListeningField.Bind(serializedObject);
			onInvokedField.Bind(serializedObject);

			targetEventField.RegisterValueChangeCallback(_ =>
			{
				UpdateVisibility();
			});

			for (int i = 0; i < SPACES_COUNT; i++)
			{
				spaces[i] = GetSpace();
			}

			UpdateVisibility();

			root.Add(targetEventField);
			root.Add(spaces[0]);
			root.Add(startListeningField);
			root.Add(stopListeningField);
			root.Add(spaces[1]);
			root.Add(onInvokedField);

			return root;
		}

		private void UpdateVisibility()
		{
			bool hasValue = targetEvent.objectReferenceValue != null;

			startListeningField.SetVisibility(hasValue);
			stopListeningField.SetVisibility(hasValue);
			onInvokedField.SetVisibility(hasValue);
			
			for (int i = 0; i < SPACES_COUNT; i++)
			{
				spaces[i].SetVisibility(hasValue);
			}
		}

		private static VisualElement GetSpace(float height = 8f)
		{
			return new VisualElement { style = { height = height } };
		}
	}
}