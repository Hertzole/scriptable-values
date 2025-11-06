using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
    [CustomEditor(typeof(ScriptableComponentPool<>), true)]
    public class ScriptableComponentPoolEditor : ScriptablePoolEditor
    {
        private SerializedProperty prefab = null!;

        protected override void GatherProperties()
        {
            base.GatherProperties();
            prefab = serializedObject.MustFindProperty(nameof(prefab));
        }

        protected override void CreateGUIBeforeStackTraces(VisualElement root)
        {
            PropertyField prefabField = new PropertyField(prefab);
            prefabField.Bind(serializedObject);

            root.Add(prefabField);

            base.CreateGUIBeforeStackTraces(root);
        }

        protected override void GetExcludingProperties(List<SerializedProperty> properties)
        {
            base.GetExcludingProperties(properties);
            properties.Add(prefab);
        }
    }
}