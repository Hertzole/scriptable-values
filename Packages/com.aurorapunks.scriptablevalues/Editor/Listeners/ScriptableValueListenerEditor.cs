﻿using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableValueListener<>), true)]
	public class ScriptableValueListenerEditor : UnityEditor.Editor
	{
		private Label valueLabel;
		private PropertyField valueField;
		private PropertyField startListeningField;
		private PropertyField stopListeningField;
		private PropertyField invokeOnField;
		private PropertyField fromValueField;
		private PropertyField toValueField;
		private PropertyField invokeParametersField;
		private PropertyField onValueChangingSingleField;
		private PropertyField onValueChangedSingleField;
		private PropertyField onValueChangingMultipleField;
		private PropertyField onValueChangedMultipleField;

		private SerializedProperty value;
		private SerializedProperty startListening;
		private SerializedProperty stopListening;
		private SerializedProperty invokeOn;
		private SerializedProperty fromValue;
		private SerializedProperty toValue;
		private SerializedProperty invokeParameters;
		private SerializedProperty onValueChangingSingle;
		private SerializedProperty onValueChangedSingle;
		private SerializedProperty onValueChangingMultiple;
		private SerializedProperty onValueChangedMultiple;

		private static readonly string[] valueFieldLabelClasses =
		{
			"unity-text-element",
			"unity-label",
			"unity-object-field-display__label"
		};

		[CanBeNull]
		private Type valueType;

		private readonly VisualElement[] spaces = new VisualElement[SPACES_COUNT];

		private const int SPACES_COUNT = 4;

		protected virtual void OnEnable()
		{
			value = serializedObject.FindProperty(nameof(value));
			startListening = serializedObject.FindProperty(nameof(startListening));
			stopListening = serializedObject.FindProperty(nameof(stopListening));
			invokeOn = serializedObject.FindProperty(nameof(invokeOn));
			fromValue = serializedObject.FindProperty(nameof(fromValue));
			toValue = serializedObject.FindProperty(nameof(toValue));
			invokeParameters = serializedObject.FindProperty(nameof(invokeParameters));
			onValueChangingSingle = serializedObject.FindProperty(nameof(onValueChangingSingle));
			onValueChangedSingle = serializedObject.FindProperty(nameof(onValueChangedSingle));
			onValueChangingMultiple = serializedObject.FindProperty(nameof(onValueChangingMultiple));
			onValueChangedMultiple = serializedObject.FindProperty(nameof(onValueChangedMultiple));

			Type baseType = target.GetType().BaseType;
			if (baseType != null && baseType.GenericTypeArguments.Length > 0)
			{
				valueType = baseType.GenericTypeArguments[0];
			}
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new VisualElement();

			valueField = new PropertyField(value);
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

			valueField.RegisterCallback<GeometryChangedEvent>(OnValueFieldGeometryChanged);

			valueField.RegisterCallback<ChangeEvent<string>>(OnValueStringChanged);

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

			valueField.RegisterValueChangeCallback(_ => UpdateVisibility());

			invokeOnField.RegisterValueChangeCallback(_ => UpdateVisibility());

			invokeParametersField.RegisterValueChangeCallback(_ => UpdateVisibility());

			for (int i = 0; i < SPACES_COUNT; i++)
			{
				spaces[i] = GetSpace();
			}

			UpdateVisibility();
			UpdateValueFieldLabel();

			root.Add(valueField);
			root.Add(spaces[0]);
			root.Add(startListeningField);
			root.Add(stopListeningField);
			root.Add(spaces[1]);
			root.Add(invokeOnField);
			root.Add(fromValueField);
			root.Add(toValueField);
			root.Add(spaces[2]);
			root.Add(invokeParametersField);
			root.Add(spaces[3]);
			root.Add(onValueChangingSingleField);
			root.Add(onValueChangedSingleField);
			root.Add(onValueChangingMultipleField);
			root.Add(onValueChangedMultipleField);

			return root;
		}

		private void UpdateVisibility()
		{
			bool hasValue = value.objectReferenceValue != null;
			bool showFromValue = invokeOn.enumValueIndex == (int) InvokeEvents.FromValue || invokeOn.enumValueIndex == (int) InvokeEvents.FromValueToValue;
			bool showToValue = invokeOn.enumValueIndex == (int) InvokeEvents.ToValue || invokeOn.enumValueIndex == (int) InvokeEvents.FromValueToValue;
			bool showSingleEvent = invokeParameters.enumValueFlag == (int) InvokeParameters.Single || invokeParameters.enumValueFlag == (int) InvokeParameters.Both;
			bool showMultipleEvent = invokeParameters.enumValueFlag == (int) InvokeParameters.Multiple || invokeParameters.enumValueFlag == (int) InvokeParameters.Both;

			SetVisibility(startListeningField, hasValue);
			SetVisibility(stopListeningField, hasValue);
			SetVisibility(invokeOnField, hasValue);
			SetVisibility(fromValueField, hasValue && showFromValue);
			SetVisibility(toValueField, hasValue && showToValue);
			SetVisibility(invokeParametersField, hasValue);
			SetVisibility(onValueChangingSingleField, hasValue && showSingleEvent);
			SetVisibility(onValueChangedSingleField, hasValue && showSingleEvent);
			SetVisibility(onValueChangingMultipleField, hasValue && showMultipleEvent);
			SetVisibility(onValueChangedMultipleField, hasValue && showMultipleEvent);

			for (int i = 0; i < SPACES_COUNT; i++)
			{
				SetVisibility(spaces[i], hasValue);
			}
		}

		private void OnValueFieldGeometryChanged(GeometryChangedEvent evt)
		{
			valueLabel ??= valueField.Q<Label>(classes: valueFieldLabelClasses);

			UpdateValueFieldLabel();
		}

		private void OnValueStringChanged(ChangeEvent<string> evt)
		{
			// Because Unity is weird, we need to update the label here too if a string changes. 
			// This fixes the issue where the label would not update when the component was first added.
			if (evt.newValue.StartsWith("None") && valueLabel != null && valueType != null)
			{
				valueLabel.text = $"None (Scriptable Value<{valueType.Name}>)";
			}
		}

		private void UpdateValueFieldLabel()
		{
			if (valueType == null || valueLabel == null || value.objectReferenceValue != null)
			{
				return;
			}

			valueLabel.text = $"None (Scriptable Value<{valueType.Name}>)";
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