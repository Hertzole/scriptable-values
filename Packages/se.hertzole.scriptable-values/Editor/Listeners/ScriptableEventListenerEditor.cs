#nullable enable

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableEventListener))]
	public class ScriptableEventListenerEditor : UnityEditor.Editor
	{
		private PropertyField targetEventField = null!;
		private PropertyField startListeningField = null!;
		private PropertyField stopListeningField = null!;
		private PropertyField onInvokedField = null!;

		private SerializedProperty targetEvent = null!;
		private SerializedProperty startListening = null!;
		private SerializedProperty stopListening = null!;
		private SerializedProperty onInvoked = null!;

		private static readonly EventCallback<SerializedPropertyChangeEvent, ScriptableEventListenerEditor> updateVisibilityCallback = static (_, args) =>
		{
			args.UpdateVisibility();
		};

		protected virtual void OnEnable()
		{
			targetEvent = serializedObject.MustFindProperty(nameof(targetEvent));
			startListening = serializedObject.MustFindProperty(nameof(startListening));
			stopListening = serializedObject.MustFindProperty(nameof(stopListening));
			onInvoked = serializedObject.MustFindProperty(nameof(onInvoked));
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

			targetEventField.RegisterValueChangeCallback(updateVisibilityCallback, this);

			UpdateVisibility();

			root.Add(targetEventField);
			root.Add(startListeningField.AddSpace());
			root.Add(stopListeningField);
			root.Add(onInvokedField.AddSpace());

			return root;
		}

		private void UpdateVisibility()
		{
			bool hasValue = targetEvent.objectReferenceValue != null;

			startListeningField.SetVisibility(hasValue);
			stopListeningField.SetVisibility(hasValue);
			onInvokedField.SetVisibility(hasValue);
		}
	}
}