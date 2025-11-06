#nullable enable

using System;
using UnityEditor;

namespace Hertzole.ScriptableValues.Editor
{
    internal static class SerializedPropertyExtensions
    {
        public static SerializedProperty MustFindProperty(this SerializedObject target, string name)
        {
            SerializedProperty property = target.FindProperty(name);
            if (property == null)
            {
                throw new Exception($"Could not find property '{name}' in {target.targetObject}.");
            }

            return property;
        }

        public static SerializedProperty MustFindPropertyRelative(this SerializedProperty property, string name)
        {
            SerializedProperty relative = property.FindPropertyRelative(name);
            if (relative == null)
            {
                throw new Exception($"Could not find property '{name}' in {property.propertyPath}.");
            }

            return relative;
        }
    }
}