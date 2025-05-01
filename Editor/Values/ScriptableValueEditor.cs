#nullable enable

using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableValue<>), true)]
	public class ScriptableValueEditor : RuntimeScriptableObjectEditor
	{
		private PropertyField? valueField;
		private PropertyField isReadOnlyField = null!;
		private PropertyField defaultValueField = null!;
		private PropertyField resetValueOnStartField = null!;
		private PropertyField setEqualityCheckField = null!;
		private PropertyField onValueChangingField = null!;
		private PropertyField onValueChangedField = null!;

		private SerializedProperty? value;
		private SerializedProperty isReadOnly = null!;
		private SerializedProperty defaultValue = null!;
		private SerializedProperty resetValueOnStart = null!;
		private SerializedProperty setEqualityCheck = null!;
		private SerializedProperty onValueChanging = null!;
		private SerializedProperty onValueChanged = null!;
		private SerializedProperty collectStackTraces = null!;

		protected override string StackTracesLabel
		{
			get { return "Set Value Stack Traces"; }
		}

		private FieldInfo valueFieldInfo = null!;

		protected override void OnEnable()
		{
			base.OnEnable();

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

			valueFieldInfo = target.GetType().GetField("value", BindingFlags.Instance | BindingFlags.NonPublic)!;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		protected override void GatherProperties()
		{
			value = serializedObject.MustFindProperty(nameof(value));
			isReadOnly = serializedObject.MustFindProperty(nameof(isReadOnly));
			defaultValue = serializedObject.MustFindProperty(nameof(defaultValue));
			resetValueOnStart = serializedObject.MustFindProperty(nameof(resetValueOnStart));
			setEqualityCheck = serializedObject.MustFindProperty(nameof(setEqualityCheck));
			onValueChanging = serializedObject.MustFindProperty(nameof(onValueChanging));
			onValueChanged = serializedObject.MustFindProperty(nameof(onValueChanged));
			collectStackTraces = serializedObject.MustFindProperty(nameof(collectStackTraces));
		}

		protected override void CreateGUIBeforeStackTraces(VisualElement root)
		{
			TextElementField? valueLabel = null;

			if (value == null || string.IsNullOrEmpty(value.propertyPath))
			{
				object currentValue = valueFieldInfo.GetValue(target);

				valueLabel = new TextElementField("Value")
				{
					value = currentValue == null ? "null" : currentValue.ToString(),
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
			// Find the isReadOnly property again because it may have been disposed.
			// The property is always disposed when exiting play mode.
			isReadOnly = serializedObject.FindProperty(nameof(isReadOnly));

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
			if (value != null)
			{
				properties.Add(value);
			}

			properties.Add(isReadOnly);
			properties.Add(defaultValue);
			properties.Add(resetValueOnStart);
			properties.Add(setEqualityCheck);
			properties.Add(onValueChanging);
			properties.Add(onValueChanged);
		}
	}
}