using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableEventListener<>), true)]
	public class GenericScriptableEventListenerEditor : UnityEditor.Editor
	{
		private Label valueLabel;
		private PropertyField targetEventField;
		private PropertyField startListeningField;
		private PropertyField stopListeningField;
		private PropertyField invokeOnField;
		private PropertyField fromValueField;
		private PropertyField toValueField;
		private PropertyField onInvokedField;

		private SerializedProperty targetEvent;
		private SerializedProperty startListening;
		private SerializedProperty stopListening;
		private SerializedProperty invokeOn;
		private SerializedProperty fromValue;
		private SerializedProperty toValue;
		private SerializedProperty onInvoked;

		private static readonly string[] valueFieldLabelClasses =
		{
			"unity-text-element",
			"unity-label",
			"unity-object-field-display__label"
		};

		[CanBeNull]
		private Type valueType;

		private readonly VisualElement[] spaces = new VisualElement[SPACES_COUNT];

		private const int SPACES_COUNT = 3;

		protected virtual void OnEnable()
		{
			targetEvent = serializedObject.FindProperty(nameof(targetEvent));
			startListening = serializedObject.FindProperty(nameof(startListening));
			stopListening = serializedObject.FindProperty(nameof(stopListening));
			invokeOn = serializedObject.FindProperty(nameof(invokeOn));
			fromValue = serializedObject.FindProperty(nameof(fromValue));
			toValue = serializedObject.FindProperty(nameof(toValue));
			onInvoked = serializedObject.FindProperty(nameof(onInvoked));

			Type baseType = target.GetType().BaseType;
			if (baseType != null && baseType.GenericTypeArguments.Length > 0)
			{
				valueType = baseType.GenericTypeArguments[0];
			}
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

			targetEventField.RegisterCallback<GeometryChangedEvent>(OnValueFieldGeometryChanged);

			targetEventField.RegisterCallback<ChangeEvent<string>>(OnValueStringChanged);

			targetEventField.Bind(serializedObject);
			startListeningField.Bind(serializedObject);
			stopListeningField.Bind(serializedObject);
			invokeOnField.Bind(serializedObject);
			fromValueField.Bind(serializedObject);
			toValueField.Bind(serializedObject);
			onInvokedField.Bind(serializedObject);

			targetEventField.RegisterValueChangeCallback(_ => { UpdateVisibility(); });

			invokeOnField.RegisterValueChangeCallback(_ => UpdateVisibility());

			for (int i = 0; i < SPACES_COUNT; i++)
			{
				spaces[i] = GetSpace();
			}

			UpdateVisibility();
			UpdateValueFieldLabel();

			root.Add(targetEventField);
			root.Add(spaces[0]);
			root.Add(startListeningField);
			root.Add(stopListeningField);
			root.Add(spaces[1]);
			root.Add(invokeOnField);
			root.Add(fromValueField);
			root.Add(toValueField);
			root.Add(spaces[2]);
			root.Add(onInvokedField);

			return root;
		}

		private void UpdateVisibility()
		{
			bool hasValue = targetEvent.objectReferenceValue != null;
			bool showFromValue = invokeOn.enumValueIndex == (int) InvokeEvents.FromValue || invokeOn.enumValueIndex == (int) InvokeEvents.FromValueToValue;
			bool showToValue = invokeOn.enumValueIndex == (int) InvokeEvents.ToValue || invokeOn.enumValueIndex == (int) InvokeEvents.FromValueToValue;

			SetVisibility(startListeningField, hasValue);
			SetVisibility(stopListeningField, hasValue);
			SetVisibility(invokeOnField, hasValue);
			SetVisibility(fromValueField, hasValue && showFromValue);
			SetVisibility(toValueField, hasValue && showToValue);
			SetVisibility(onInvokedField, hasValue);

			for (int i = 0; i < SPACES_COUNT; i++)
			{
				SetVisibility(spaces[i], hasValue);
			}
		}

		private void OnValueFieldGeometryChanged(GeometryChangedEvent evt)
		{
			valueLabel ??= targetEventField.Q<Label>(classes: valueFieldLabelClasses);

			UpdateValueFieldLabel();
		}

		private void OnValueStringChanged(ChangeEvent<string> evt)
		{
			// Because Unity is weird, we need to update the label here too if a string changes. 
			// This fixes the issue where the label would not update when the component was first added.
			if (evt.newValue.StartsWith("None") && valueLabel != null && valueType != null)
			{
				valueLabel.text = $"None (Scriptable Event<{valueType.Name}>)";
			}
		}

		private void UpdateValueFieldLabel()
		{
			if (valueType == null || valueLabel == null || targetEvent.objectReferenceValue != null)
			{
				return;
			}

			valueLabel.text = $"None (Scriptable Event<{valueType.Name}>)";
		}

		private static VisualElement GetSpace(float height = 8f)
		{
			return new VisualElement { style = { height = height } };
		}

		private static void SetVisibility(VisualElement element, bool visible)
		{
			element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
		}
	}
}