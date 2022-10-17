using AuroraPunks.ScriptableValues.Debugging;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableList<>), true)]
	public class ScriptableListEditor : UnityEditor.Editor
	{
		private SerializedProperty setEqualityCheck;
		private SerializedProperty isReadOnly;
		private SerializedProperty list;

		private StackTraceElement stackTraces;
		private PropertyField setEqualityCheckField;
		private PropertyField isReadOnlyField;
		private PropertyField listField;

		private void OnEnable()
		{
			setEqualityCheck = serializedObject.FindProperty(nameof(setEqualityCheck));
			isReadOnly = serializedObject.FindProperty(nameof(isReadOnly));
			list = serializedObject.FindProperty(nameof(list));
			
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		private void OnDisable()
		{
			stackTraces?.Dispose();
			
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		public override VisualElement CreateInspectorGUI()
		{
			EntireInspectorElement root = new EntireInspectorElement();

			setEqualityCheckField = new PropertyField(setEqualityCheck);
			isReadOnlyField = new PropertyField(isReadOnly);
			listField = new PropertyField(list);
			
			setEqualityCheckField.Bind(serializedObject);
			isReadOnlyField.Bind(serializedObject);
			listField.Bind(serializedObject);

			stackTraces = new StackTraceElement((IStackTraceProvider) target, "List Change Stack Traces")
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

			root.Add(isReadOnlyField);
			root.Add(setEqualityCheckField);
			root.Add(listField);
			root.Add(stackTraces);

			return root;
		}
		
		private void UpdateVisibility(bool readOnly)
		{
			setEqualityCheckField.SetVisibility(!readOnly);
		}

		private void UpdateEnabledState()
		{
			bool enabled = !isReadOnly.boolValue || !EditorApplication.isPlayingOrWillChangePlaymode;
			isReadOnlyField.SetEnabled(enabled);
			listField.SetEnabled(enabled);
		}
		
		private void OnPlayModeStateChanged(PlayModeStateChange obj)
		{
			UpdateEnabledState();
		}
	}
}