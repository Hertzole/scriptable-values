using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableComponentPool<>), true)]
	public class ScriptableComponentPoolEditor : ScriptablePoolEditor
	{
		private SerializedProperty prefab;

		protected override void OnEnable()
		{
			base.OnEnable();
			prefab = serializedObject.FindProperty(nameof(prefab));
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = base.CreateInspectorGUI();

			PropertyField prefabField = new PropertyField(prefab);
			prefabField.Bind(serializedObject);

			root.Insert(0, prefabField);

			return root;
		}
	}
}