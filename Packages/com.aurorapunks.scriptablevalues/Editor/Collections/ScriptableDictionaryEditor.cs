using AuroraPunks.ScriptableValues.Debugging;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	//TODO: Actually show the dictionary... somehow.
	[CustomEditor(typeof(ScriptableDictionary<,>), true)]
	public class ScriptableDictionaryEditor : UnityEditor.Editor
	{
		private SerializedProperty setEqualityCheck;
		private SerializedProperty editorKey;
		private SerializedProperty editorValue;

		private StackTraceElement stackTraces;

		private void OnEnable()
		{
			setEqualityCheck = serializedObject.FindProperty(nameof(setEqualityCheck));
			editorKey = serializedObject.FindProperty(nameof(editorKey));
			editorValue = serializedObject.FindProperty(nameof(editorValue));
		}

		private void OnDisable()
		{
			stackTraces?.Dispose();
		}

		public override VisualElement CreateInspectorGUI()
		{
			EntireInspectorElement root = new EntireInspectorElement();

			PropertyField setEqualityCheckElement = new PropertyField(setEqualityCheck);
			setEqualityCheckElement.Bind(serializedObject);

			stackTraces = new StackTraceElement((IStackTraceProvider) target, "Dictionary Change Stack Traces")
			{
				style =
				{
					marginTop = 4
				}
			};

			root.Add(setEqualityCheckElement);
			root.Add(stackTraces);

			return root;
		}
	}
}