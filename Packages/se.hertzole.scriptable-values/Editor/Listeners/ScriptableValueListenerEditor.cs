#nullable enable

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	// This editor is mainly used for Odin Inspector to understand that it shouldn't override this inspector.
	[CustomEditor(typeof(ScriptableListenerBase), true)]
	public class ScriptableValueListenerBaseEditor : UnityEditor.Editor { }

	[CustomEditor(typeof(ScriptableValueListener<>), true)]
	public class ScriptableValueListenerEditor : ScriptableValueListenerBaseEditor
	{
		private PropertyField valueField = null!;
		private PropertyField startListeningField = null!;
		private PropertyField stopListeningField = null!;
		private PropertyField invokeOnField = null!;
		private PropertyField fromValueField = null!;
		private PropertyField toValueField = null!;
		private PropertyField invokeParametersField = null!;
		private PropertyField onValueChangingSingleField = null!;
		private PropertyField onValueChangedSingleField = null!;
		private PropertyField onValueChangingMultipleField = null!;
		private PropertyField onValueChangedMultipleField = null!;

		private SerializedProperty targetValue = null!;
		private SerializedProperty startListening = null!;
		private SerializedProperty stopListening = null!;
		private SerializedProperty invokeOn = null!;
		private SerializedProperty fromValue = null!;
		private SerializedProperty toValue = null!;
		private SerializedProperty invokeParameters = null!;
		private SerializedProperty onValueChangingSingle = null!;
		private SerializedProperty onValueChangedSingle = null!;
		private SerializedProperty onValueChangingMultiple = null!;
		private SerializedProperty onValueChangedMultiple = null!;

		private static readonly EventCallback<SerializedPropertyChangeEvent, ScriptableValueListenerEditor> updateVisibilityCallback = static (_, args) =>
		{
			args.UpdateVisibility();
		};

		protected virtual void OnEnable()
		{
			targetValue = serializedObject.MustFindProperty(nameof(targetValue));
			startListening = serializedObject.MustFindProperty(nameof(startListening));
			stopListening = serializedObject.MustFindProperty(nameof(stopListening));
			invokeOn = serializedObject.MustFindProperty(nameof(invokeOn));
			fromValue = serializedObject.MustFindProperty(nameof(fromValue));
			toValue = serializedObject.MustFindProperty(nameof(toValue));
			invokeParameters = serializedObject.MustFindProperty(nameof(invokeParameters));
			onValueChangingSingle = serializedObject.MustFindProperty(nameof(onValueChangingSingle));
			onValueChangedSingle = serializedObject.MustFindProperty(nameof(onValueChangedSingle));
			onValueChangingMultiple = serializedObject.MustFindProperty(nameof(onValueChangingMultiple));
			onValueChangedMultiple = serializedObject.MustFindProperty(nameof(onValueChangedMultiple));
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new VisualElement();

			valueField = new PropertyField(targetValue);
			startListeningField = new PropertyField(startListening);
			stopListeningField = new PropertyField(stopListening);
			invokeOnField = new PropertyField(invokeOn);
			fromValueField = new PropertyField(fromValue);
			toValueField = new PropertyField(toValue);
			invokeParametersField = new PropertyField(invokeParameters);
			onValueChangingSingleField = new PropertyField(onValueChangingSingle);
			onValueChangedSingleField = new PropertyField(onValueChangedSingle);
			onValueChangingMultipleField = new PropertyField(onValueChangingMultiple);
			onValueChangedMultipleField = new PropertyField(onValueChangedMultiple);

			valueField.Bind(serializedObject);
			startListeningField.Bind(serializedObject);
			stopListeningField.Bind(serializedObject);
			invokeOnField.Bind(serializedObject);
			fromValueField.Bind(serializedObject);
			toValueField.Bind(serializedObject);
			invokeParametersField.Bind(serializedObject);
			onValueChangingSingleField.Bind(serializedObject);
			onValueChangedSingleField.Bind(serializedObject);
			onValueChangingMultipleField.Bind(serializedObject);
			onValueChangedMultipleField.Bind(serializedObject);

			valueField.RegisterValueChangeCallback(updateVisibilityCallback, this);
			invokeOnField.RegisterValueChangeCallback(updateVisibilityCallback, this);
			invokeParametersField.RegisterValueChangeCallback(updateVisibilityCallback, this);

			invokeParametersField.style.marginBottom = 8;

			UpdateVisibility();

			root.Add(valueField);
			root.Add(startListeningField.AddSpace());
			root.Add(stopListeningField);
			root.Add(invokeOnField.AddSpace());
			root.Add(fromValueField);
			root.Add(toValueField);
			root.Add(invokeParametersField.AddSpace());
			root.Add(onValueChangingSingleField);
			root.Add(onValueChangedSingleField);
			root.Add(onValueChangingMultipleField);
			root.Add(onValueChangedMultipleField);

			return root;
		}

		private void UpdateVisibility()
		{
			bool hasValue = targetValue.objectReferenceValue != null;
			bool showFromValue = invokeOn.enumValueIndex == (int) InvokeEvents.FromValue || invokeOn.enumValueIndex == (int) InvokeEvents.FromValueToValue;
			bool showToValue = invokeOn.enumValueIndex == (int) InvokeEvents.ToValue || invokeOn.enumValueIndex == (int) InvokeEvents.FromValueToValue;
			bool showSingleEvent = invokeParameters.enumValueFlag == (int) InvokeParameters.Single ||
			                       invokeParameters.enumValueFlag == (int) InvokeParameters.Both;

			bool showMultipleEvent = invokeParameters.enumValueFlag == (int) InvokeParameters.Multiple ||
			                         invokeParameters.enumValueFlag == (int) InvokeParameters.Both;

			startListeningField.SetVisibility(hasValue);

			startListeningField.SetVisibility(hasValue);
			stopListeningField.SetVisibility(hasValue);
			invokeOnField.SetVisibility(hasValue);
			fromValueField.SetVisibility(hasValue && showFromValue);
			toValueField.SetVisibility(hasValue && showToValue);
			invokeParametersField.SetVisibility(hasValue);
			onValueChangingSingleField.SetVisibility(hasValue && showSingleEvent);
			onValueChangedSingleField.SetVisibility(hasValue && showSingleEvent);
			onValueChangingMultipleField.SetVisibility(hasValue && showMultipleEvent);
			onValueChangedMultipleField.SetVisibility(hasValue && showMultipleEvent);
		}
	}
}