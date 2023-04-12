﻿using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableValue<>), true)]
	public class ScriptableValueEditor : RuntimeScriptableObjectEditor
	{
		private PropertyField valueField;
		private PropertyField isReadOnlyField;
		private PropertyField defaultValueField;
		private PropertyField resetValueOnStartField;
		private PropertyField setEqualityCheckField;
		private PropertyField onValueChangingField;
		private PropertyField onValueChangedField;
		private SerializedProperty value;
		private SerializedProperty isReadOnly;
		private SerializedProperty defaultValue;
		private SerializedProperty resetValueOnStart;
		private SerializedProperty setEqualityCheck;
		private SerializedProperty onValueChanging;
		private SerializedProperty onValueChanged;
		private SerializedProperty collectStackTraces;

		protected override string StackTracesLabel { get { return "Set Value Stack Traces"; } }

		private FieldInfo valueFieldInfo;

		protected override void OnEnable()
		{
			base.OnEnable();

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			
			valueFieldInfo = target.GetType().GetField("value", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		protected override void GatherProperties()
		{
			value = serializedObject.FindProperty(nameof(value));
			isReadOnly = serializedObject.FindProperty(nameof(isReadOnly));
			defaultValue = serializedObject.FindProperty(nameof(defaultValue));
			resetValueOnStart = serializedObject.FindProperty(nameof(resetValueOnStart));
			setEqualityCheck = serializedObject.FindProperty(nameof(setEqualityCheck));
			onValueChanging = serializedObject.FindProperty(nameof(onValueChanging));
			onValueChanged = serializedObject.FindProperty(nameof(onValueChanged));
			collectStackTraces = serializedObject.FindProperty(nameof(collectStackTraces));
		}

		protected override void CreateGUIBeforeStackTraces(VisualElement root)
		{
			TextElementField valueLabel = null;
			
			if (value == null || string.IsNullOrEmpty(value.propertyPath))
			{
				valueLabel = new TextElementField("Value")
				{
					value = valueFieldInfo.GetValue(target).ToString(),
					style =
					{
						marginBottom = 16
					}
				};
				valueLabel.AddToClassList(TextElementField.alignedFieldUssClassName);
			}
			else
			{
				valueField = new PropertyField(value)
				{
					style =
					{
						marginBottom = 16
					}
				};
			}
			isReadOnlyField = new PropertyField(isReadOnly);
			defaultValueField = new PropertyField(defaultValue);
			resetValueOnStartField = new PropertyField(resetValueOnStart);
			setEqualityCheckField = new PropertyField(setEqualityCheck);
			onValueChangingField = new PropertyField(onValueChanging);
			onValueChangedField = new PropertyField(onValueChanged);

			valueField?.Bind(serializedObject);
			isReadOnlyField.Bind(serializedObject);
			defaultValueField.Bind(serializedObject);
			resetValueOnStartField.Bind(serializedObject);
			setEqualityCheckField.Bind(serializedObject);
			onValueChangingField.Bind(serializedObject);
			onValueChangedField.Bind(serializedObject);

			isReadOnlyField.RegisterValueChangeCallback(evt => { UpdateVisibility(evt.changedProperty.boolValue); });

			UpdateVisibility(isReadOnly.boolValue);
			UpdateEnabledState();

			if (valueField != null)
			{
				root.Add(valueField);
			}
			else
			{
				root.Add(valueLabel);
			}
			root.Add(isReadOnlyField);
			root.Add(defaultValueField);
			root.Add(resetValueOnStartField);
			root.Add(setEqualityCheckField);
			
			CreateDefaultInspectorGUI(root);
			
			root.Add(onValueChangingField);
			root.Add(onValueChangedField);
		}

		private void UpdateVisibility(bool readOnly)
		{
			defaultValueField.SetVisibility(!readOnly);
			resetValueOnStartField.SetVisibility(!readOnly);
			setEqualityCheckField.SetVisibility(!readOnly);
			onValueChangingField.SetVisibility(!readOnly);
			onValueChangedField.SetVisibility(!readOnly);
		}

		private void UpdateEnabledState()
		{
			bool enabled = !isReadOnly.boolValue || !EditorApplication.isPlayingOrWillChangePlaymode;
			valueField?.SetEnabled(enabled);
			isReadOnlyField.SetEnabled(enabled);
		}

		private void OnPlayModeStateChanged(PlayModeStateChange obj)
		{
			UpdateEnabledState();
		}

		protected override void GetExcludingProperties(List<SerializedProperty> properties)
		{
			properties.Add(value);
			properties.Add(isReadOnly);
			properties.Add(defaultValue);
			properties.Add(resetValueOnStart);
			properties.Add(setEqualityCheck);
			properties.Add(onValueChanging);
			properties.Add(onValueChanged);
		}
	}
}