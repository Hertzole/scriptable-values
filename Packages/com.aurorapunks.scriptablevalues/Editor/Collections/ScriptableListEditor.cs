using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableList<>), true)]
	public class ScriptableListEditor : RuntimeScriptableObjectEditor
	{
		private PropertyField setEqualityCheckField;
		private PropertyField isReadOnlyField;
		private PropertyField clearOnStartField;
		private PropertyField listField;
		private SerializedProperty setEqualityCheck;
		private SerializedProperty isReadOnly;
		private SerializedProperty clearOnStart;
		private SerializedProperty list;

		protected override string StackTracesLabel { get { return "List Change Stack Traces"; } }

		protected override void OnEnable()
		{
			base.OnEnable();

			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		protected override void GatherProperties()
		{
			setEqualityCheck = serializedObject.FindProperty(nameof(setEqualityCheck));
			isReadOnly = serializedObject.FindProperty(nameof(isReadOnly));
			clearOnStart = serializedObject.FindProperty(nameof(clearOnStart));
			list = serializedObject.FindProperty(nameof(list));
		}

		protected override void CreateGUIBeforeStackTraces(VisualElement root)
		{
			setEqualityCheckField = new PropertyField(setEqualityCheck);
			isReadOnlyField = new PropertyField(isReadOnly);
			clearOnStartField = new PropertyField(clearOnStart);
			listField = new PropertyField(list);

			setEqualityCheckField.Bind(serializedObject);
			isReadOnlyField.Bind(serializedObject);
			clearOnStartField.Bind(serializedObject);
			listField.Bind(serializedObject);

			isReadOnlyField.RegisterValueChangeCallback(evt => { UpdateVisibility(evt.changedProperty.boolValue); });

			UpdateVisibility(isReadOnly.boolValue);
			UpdateEnabledState();

			root.Add(isReadOnlyField);
			root.Add(setEqualityCheckField);
			root.Add(clearOnStartField);

			CreateDefaultInspectorGUI(root);
			
			root.Add(listField);
		}

		private void UpdateVisibility(bool readOnly)
		{
			setEqualityCheckField.SetVisibility(!readOnly);
			clearOnStartField.SetVisibility(!readOnly);
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

		protected override void GetExcludingProperties(List<SerializedProperty> properties)
		{
			properties.Add(setEqualityCheck);
			properties.Add(isReadOnly);
			properties.Add(clearOnStart);
			properties.Add(list);
		}
	}
}