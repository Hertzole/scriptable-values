using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableComponentPool<>), true)]
	public class ScriptableComponentPoolEditor : ScriptablePoolEditor
	{
		private SerializedProperty prefab;

		protected override void GatherProperties()
		{
			base.GatherProperties();
			prefab = serializedObject.FindProperty(nameof(prefab));
		}

		protected override void CreateGUIBeforeStackTraces(VisualElement root)
		{
			PropertyField prefabField = new PropertyField(prefab);
			prefabField.Bind(serializedObject);

			root.Add(prefabField);

			base.CreateGUIBeforeStackTraces(root);
		}
	}
}