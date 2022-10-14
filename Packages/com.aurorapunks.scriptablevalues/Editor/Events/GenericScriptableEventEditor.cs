using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableEvent<>), true)]
	public class GenericScriptableEventEditor : ScriptableEventEditor
	{
		private SerializedProperty onInvokedWithArgs;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			
			onInvokedWithArgs = serializedObject.FindProperty(nameof(onInvokedWithArgs));
		}

		public override VisualElement CreateInspectorGUI()
		{
			var root = base.CreateInspectorGUI();

			var onInvokedWithArgsField = new PropertyField(onInvokedWithArgs);
			onInvokedWithArgsField.Bind(serializedObject);
			root.Insert(1, onInvokedWithArgsField);
			
			return root;
		}
	}
}