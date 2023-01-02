using UnityEditor;
using AuroraPunks.ScriptableValues.Debugging;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableValue<>), true)]
	public class ScriptableValueEditor : UnityEditor.Editor
	{
		private SerializedProperty value;
		private SerializedProperty isReadOnly;
		private SerializedProperty defaultValue;
		private SerializedProperty resetValueOnStart;
		private SerializedProperty setEqualityCheck;
		private SerializedProperty onValueChanging;
		private SerializedProperty onValueChanged;
		private SerializedProperty collectStackTraces;

		private StackTraceElement stackTraces;
		private PropertyField valueField;
		private PropertyField isReadOnlyField;
		private PropertyField defaultValueField;
		private PropertyField resetValueOnStartField;
		private PropertyField setEqualityCheckField;
		private PropertyField onValueChangingField;
		private PropertyField onValueChangedField;

		protected virtual void OnEnable()
		{
			value = serializedObject.FindProperty(nameof(value));
			isReadOnly = serializedObject.FindProperty(nameof(isReadOnly));
			defaultValue = serializedObject.FindProperty(nameof(defaultValue));
			resetValueOnStart = serializedObject.FindProperty(nameof(resetValueOnStart));
			setEqualityCheck = serializedObject.FindProperty(nameof(setEqualityCheck));
			onValueChanging = serializedObject.FindProperty(nameof(onValueChanging));
			onValueChanged = serializedObject.FindProperty(nameof(onValueChanged));
			collectStackTraces = serializedObject.FindProperty(nameof(collectStackTraces));
			
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		protected virtual void OnDisable()
		{
			stackTraces?.Dispose();
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new EntireInspectorElement();

			valueField = new PropertyField(value);
			isReadOnlyField = new PropertyField(isReadOnly);
			defaultValueField = new PropertyField(defaultValue);
			resetValueOnStartField = new PropertyField(resetValueOnStart);
			setEqualityCheckField = new PropertyField(setEqualityCheck);
			onValueChangingField = new PropertyField(onValueChanging);
			onValueChangedField = new PropertyField(onValueChanged);

			valueField.Bind(serializedObject);
			isReadOnlyField.Bind(serializedObject);
			defaultValueField.Bind(serializedObject);
			resetValueOnStartField.Bind(serializedObject);
			setEqualityCheckField.Bind(serializedObject);
			onValueChangingField.Bind(serializedObject);
			onValueChangedField.Bind(serializedObject);

			stackTraces = new StackTraceElement((IStackTraceProvider) target, collectStackTraces, "Set Value Stack Traces")
			{
				style =
				{
					marginTop = 4
				}
			};
			
			isReadOnlyField.RegisterValueChangeCallback(evt =>
			{
				UpdateVisibility(evt.changedProperty.boolValue);
			});
			
			UpdateVisibility(isReadOnly.boolValue);
			UpdateEnabledState();

			root.Add(valueField);
			root.Add(isReadOnlyField);
			root.Add(defaultValueField);
			root.Add(resetValueOnStartField);
			root.Add(setEqualityCheckField);
			root.Add(onValueChangingField);
			root.Add(onValueChangedField);
			root.Add(stackTraces);

			return root;
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
			valueField.SetEnabled(enabled);
			isReadOnlyField.SetEnabled(enabled);
		}
		
		private void OnPlayModeStateChanged(PlayModeStateChange obj)
		{
			UpdateEnabledState();
		}
	}
}