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
		private SerializedProperty list;

		private StackTraceElement stackTraces;

		private void OnEnable()
		{
			setEqualityCheck = serializedObject.FindProperty(nameof(setEqualityCheck));
			list = serializedObject.FindProperty(nameof(list));
		}

		private void OnDisable()
		{
			stackTraces?.Dispose();
		}

		public override VisualElement CreateInspectorGUI()
		{
			EntireInspectorElement root = new EntireInspectorElement();

			PropertyField setEqualityCheckField = new PropertyField(setEqualityCheck);
			setEqualityCheckField.Bind(serializedObject);
			PropertyField listField = new PropertyField(list);
			listField.Bind(serializedObject);

			stackTraces = new StackTraceElement((IStackTraceProvider) target, "List Change Stack Traces")
			{
				style =
				{
					marginTop = 4
				}
			};

			root.Add(setEqualityCheckField);
			root.Add(listField);
			root.Add(stackTraces);

			return root;
		}
	}
}