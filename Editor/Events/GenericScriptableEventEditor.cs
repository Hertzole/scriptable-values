#nullable enable

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
    [CustomEditor(typeof(ScriptableEvent<>), true)]
    public class GenericScriptableEventEditor : RuntimeScriptableObjectEditor
    {
        private MethodInfo invokeMethod = null!;
        private SerializedProperty onInvokedWithArgs = null!;
        private SerializedProperty? editorInvokeValue;

        protected override void GatherProperties()
        {
            base.GatherProperties();

            onInvokedWithArgs = serializedObject.FindProperty(nameof(onInvokedWithArgs));
            editorInvokeValue = serializedObject.FindProperty(nameof(editorInvokeValue));

            Type? baseType = target.GetType().BaseType;
            if (baseType != null)
            {
                invokeMethod = baseType.GetMethod("Invoke", new[] { typeof(object), baseType.GenericTypeArguments[0] })!;
                Assert.IsNotNull(invokeMethod);
            }
        }

        protected override void CreateGUIBeforeStackTraces(VisualElement root)
        {
            base.CreateGUIBeforeStackTraces(root);

            VisualElement invokeElement = CreateInvokeButton();
            invokeElement.style.marginBottom = 8;
            root.Add(invokeElement);

            PropertyField onInvokedWithArgsField = new PropertyField(onInvokedWithArgs);
            onInvokedWithArgsField.Bind(serializedObject);
            root.Add(onInvokedWithArgsField);
        }

        private VisualElement CreateInvokeButton()
        {
            VisualElement root = new VisualElement();

            if (editorInvokeValue != null)
            {
                PropertyField editorInvokeValueField = new PropertyField(editorInvokeValue, "Invoke Value");
                editorInvokeValueField.Bind(serializedObject);
                root.Add(editorInvokeValueField);

                Button invokeButton = new Button(() => { OnClickInvoke(GetInvokeValue()); })
                {
                    text = "Invoke With Args"
                };

                root.Add(invokeButton);
            }
            else
            {
                root.Add(new HelpBox("Type is not serializable and can not be invoked from the inspector.", HelpBoxMessageType.Info));
            }

            return root;
        }

        private object? GetInvokeValue()
        {
            Type? targetObject = target.GetType().BaseType;
            if (targetObject == null)
            {
                return null;
            }

            FieldInfo? targetField = targetObject.GetField("editorInvokeValue", BindingFlags.Instance | BindingFlags.NonPublic);

            if (targetField == null)
            {
                return null;
            }

            return targetField.GetValue(target);
        }

        private void OnClickInvoke(object? args)
        {
            invokeMethod.Invoke(target, new[] { this, args });
        }

        protected override void GetExcludingProperties(List<SerializedProperty> properties)
        {
            base.GetExcludingProperties(properties);
            properties.Add(onInvokedWithArgs);

            if (editorInvokeValue != null)
            {
                properties.Add(editorInvokeValue);
            }
        }
    }
}