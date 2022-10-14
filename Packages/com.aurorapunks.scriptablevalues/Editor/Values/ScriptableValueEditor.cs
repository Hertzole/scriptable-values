using AuroraPunks.ScriptableValues.Debugging;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableValue<>), true)]
	public class ScriptableValueEditor : UnityEditor.Editor
	{
		private SerializedProperty value;
		private SerializedProperty defaultValue;
		private SerializedProperty resetValueOnStart;
		private SerializedProperty setEqualityCheck;
		private SerializedProperty onValueChanging;
		private SerializedProperty onValueChanged;

		private StackTraceElement stackTraces;

		protected virtual void OnEnable()
		{
			value = serializedObject.FindProperty(nameof(value));
			defaultValue = serializedObject.FindProperty(nameof(defaultValue));
			resetValueOnStart = serializedObject.FindProperty(nameof(resetValueOnStart));
			setEqualityCheck = serializedObject.FindProperty(nameof(setEqualityCheck));
			onValueChanging = serializedObject.FindProperty(nameof(onValueChanging));
			onValueChanged = serializedObject.FindProperty(nameof(onValueChanged));
		}

		protected virtual void OnDisable()
		{
			stackTraces?.Dispose();
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new EntireInspectorElement();

			PropertyField valueField = new PropertyField(value);
			valueField.Bind(serializedObject);
			PropertyField defaultValueField = new PropertyField(defaultValue);
			defaultValueField.Bind(serializedObject);
			PropertyField resetValueOnStartField = new PropertyField(resetValueOnStart);
			resetValueOnStartField.Bind(serializedObject);
			PropertyField setEqualityCheckField = new PropertyField(setEqualityCheck);
			setEqualityCheckField.Bind(serializedObject);
			PropertyField onValueChangingField = new PropertyField(onValueChanging);
			onValueChangingField.Bind(serializedObject);
			PropertyField onValueChangedField = new PropertyField(onValueChanged);
			onValueChangedField.Bind(serializedObject);

			stackTraces = new StackTraceElement((IStackTraceProvider) target, "Set Value Stack Traces")
			{
				style =
				{
					marginTop = 4
				}
			};

			root.Add(valueField);
			root.Add(defaultValueField);
			root.Add(resetValueOnStartField);
			root.Add(setEqualityCheckField);
			root.Add(onValueChangingField);
			root.Add(onValueChangedField);
			root.Add(stackTraces);

			return root;
		}
	}
}