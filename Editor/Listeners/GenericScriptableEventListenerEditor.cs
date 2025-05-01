#nullable enable

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableEventListener<>), true)]
	public class GenericScriptableEventListenerEditor : UnityEditor.Editor
	{
		private PropertyField targetEventField = null!;
		private PropertyField startListeningField = null!;
		private PropertyField stopListeningField = null!;
		private PropertyField invokeOnField = null!;
		private PropertyField fromValueField = null!;
		private PropertyField toValueField = null!;
		private PropertyField onInvokedField = null!;

		private SerializedProperty targetEvent = null!;
		private SerializedProperty startListening = null!;
		private SerializedProperty stopListening = null!;
		private SerializedProperty invokeOn = null!;
		private SerializedProperty fromValue = null!;
		private SerializedProperty toValue = null!;
		private SerializedProperty onInvoked = null!;

		protected virtual void OnEnable()
		{
			targetEvent = serializedObject.MustFindProperty(nameof(targetEvent));
			startListening = serializedObject.MustFindProperty(nameof(startListening));
			stopListening = serializedObject.MustFindProperty(nameof(stopListening));
			invokeOn = serializedObject.MustFindProperty(nameof(invokeOn));
			fromValue = serializedObject.MustFindProperty(nameof(fromValue));
			toValue = serializedObject.MustFindProperty(nameof(toValue));
			onInvoked = serializedObject.MustFindProperty(nameof(onInvoked));
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new VisualElement();

			targetEventField = new PropertyField(targetEvent);
			startListeningField = new PropertyField(startListening);
			stopListeningField = new PropertyField(stopListening);
			invokeOnField = new PropertyField(invokeOn);
			fromValueField = new PropertyField(fromValue);
			toValueField = new PropertyField(toValue);
			onInvokedField = new PropertyField(onInvoked);

			targetEventField.Bind(serializedObject);
			startListeningField.Bind(serializedObject);
			stopListeningField.Bind(serializedObject);
			invokeOnField.Bind(serializedObject);
			fromValueField.Bind(serializedObject);
			toValueField.Bind(serializedObject);
			onInvokedField.Bind(serializedObject);

			targetEventField.RegisterValueChangeCallback(_ => { UpdateVisibility(); });
			invokeOnField.RegisterValueChangeCallback(_ => UpdateVisibility());

			UpdateVisibility();

			root.Add(targetEventField);
			root.Add(startListeningField.AddSpace());
			root.Add(stopListeningField);
			root.Add(invokeOnField.AddSpace());
			root.Add(fromValueField);
			root.Add(toValueField);
			root.Add(onInvokedField.AddSpace());

			return root;
		}

		private void UpdateVisibility()
		{
			bool hasValue = targetEvent.objectReferenceValue != null;
			bool showFromValue = invokeOn.enumValueIndex == (int) InvokeEvents.FromValue || invokeOn.enumValueIndex == (int) InvokeEvents.FromValueToValue;
			bool showToValue = invokeOn.enumValueIndex == (int) InvokeEvents.ToValue || invokeOn.enumValueIndex == (int) InvokeEvents.FromValueToValue;

			startListeningField.SetVisibility(hasValue);
			stopListeningField.SetVisibility(hasValue);
			invokeOnField.SetVisibility(hasValue);
			fromValueField.SetVisibility(hasValue && showFromValue);
			toValueField.SetVisibility(hasValue && showToValue);
			onInvokedField.SetVisibility(hasValue);
		}
	}
}