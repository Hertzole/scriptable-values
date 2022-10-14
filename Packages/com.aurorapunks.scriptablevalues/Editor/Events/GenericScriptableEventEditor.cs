using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableEvent<>), true)]
	public class GenericScriptableEventEditor : ScriptableEventEditor
	{
		private MethodInfo invokeMethod;
		private SerializedProperty onInvokedWithArgs;
		private SerializedProperty editorInvokeValue;

		protected override void OnEnable()
		{
			base.OnEnable();

			onInvokedWithArgs = serializedObject.FindProperty(nameof(onInvokedWithArgs));
			editorInvokeValue = serializedObject.FindProperty(nameof(editorInvokeValue));

			Type baseType = target.GetType().BaseType;
			if (baseType != null)
			{
				invokeMethod = baseType.GetMethod("Invoke", new[] { typeof(object), baseType.GenericTypeArguments[0] });
			}
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = base.CreateInspectorGUI();

			PropertyField onInvokedWithArgsField = new PropertyField(onInvokedWithArgs);
			onInvokedWithArgsField.Bind(serializedObject);
			root.Insert(1, onInvokedWithArgsField);

			return root;
		}

		protected override VisualElement CreateInvokeButton()
		{
			VisualElement root = new VisualElement();

			PropertyField editorInvokeValueField = new PropertyField(editorInvokeValue, "Invoke Value");
			editorInvokeValueField.Bind(serializedObject);
			root.Add(editorInvokeValueField);

			Button invokeButton = new Button(() => { OnClickInvoke(GetInvokeValue()); })
			{
				text = "Invoke With Args"
			};

			root.Add(invokeButton);

			return root;
		}

		private object GetInvokeValue()
		{
			Type targetObject = target.GetType().BaseType;
			if (targetObject == null)
			{
				return default;
			}

			FieldInfo targetField = targetObject.GetField("editorInvokeValue", BindingFlags.Instance | BindingFlags.NonPublic);

			if (targetField == null)
			{
				return default;
			}

			return targetField.GetValue(target);
		}

		private void OnClickInvoke(object args)
		{
			invokeMethod.Invoke(target, new[] { this, args });
		}
	}
}