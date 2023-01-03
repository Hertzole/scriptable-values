using AuroraPunks.ScriptableValues.Debugging;
using AuroraPunks.ScriptableValues.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues
{
	public abstract class RuntimeScriptableObjectEditor : UnityEditor.Editor
	{
		private SerializedProperty collectStackTraces;
		private StackTraceElement stackTraces;

		protected virtual string StackTracesLabel { get { return "Invocation Stack Traces"; } }

		protected virtual void OnEnable()
		{
			collectStackTraces = serializedObject.FindProperty(nameof(collectStackTraces));

			((IStackTraceProvider) target).OnStackTraceAdded += OnStackTraceAdded;

			GatherProperties();
		}

		protected virtual void OnDisable()
		{
			((IStackTraceProvider) target).OnStackTraceAdded -= OnStackTraceAdded;
			stackTraces?.Dispose();
		}

		protected virtual void GatherProperties() { }

		protected virtual void OnStackTraceAdded() { }

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new EntireInspectorElement();
			CreateGUIBeforeStackTraces(root);

			stackTraces = new StackTraceElement((IStackTraceProvider) target, collectStackTraces, StackTracesLabel)
			{
				style =
				{
					marginTop = 4
				}
			};

			root.Add(stackTraces);

			return root;
		}

		protected virtual void CreateGUIBeforeStackTraces(VisualElement root) { }
	}
}