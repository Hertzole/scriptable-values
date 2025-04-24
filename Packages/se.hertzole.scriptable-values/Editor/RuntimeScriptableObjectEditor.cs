#nullable enable

using System.Collections.Generic;
using System.Reflection;
using Hertzole.ScriptableValues.Debugging;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	[CustomEditor(typeof(RuntimeScriptableObject), true)]
	public class RuntimeScriptableObjectEditor : UnityEditor.Editor
	{
		private bool hasCreatedDefaultInspector;
		private bool hideStackTraces;

		private StackTraceElement? stackTraces = null;

		protected virtual string StackTracesLabel
		{
			get { return "Invocation Stack Traces"; }
		}

		protected virtual void OnEnable()
		{
			hasCreatedDefaultInspector = false;

			if (target.GetType().GetCustomAttribute(typeof(HideStackTracesAttribute), false) != null)
			{
				hideStackTraces = true;
			}

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
			CreateDefaultInspectorGUI(root);

			if (!hideStackTraces)
			{
				stackTraces = new StackTraceElement((IStackTraceProvider) target, StackTracesLabel)
				{
					style =
					{
						marginTop = 4
					}
				};

				root.Add(stackTraces);
			}

			return root;
		}

		protected void CreateDefaultInspectorGUI(VisualElement root)
		{
			if (hasCreatedDefaultInspector)
			{
				return;
			}

			SerializedProperty iterator = serializedObject.GetIterator();
			bool enterChildren = true;

			using PooledObject<List<SerializedProperty>> propertiesScope = ListPool<SerializedProperty>.Get(out List<SerializedProperty> ignoreProperties);
			using PooledObject<HashSet<string>> propertyNamesScope = HashSetPool<string>.Get(out HashSet<string> ignorePropertyNames);

			ignorePropertyNames.Add("m_Script");

			GetExcludingProperties(ignoreProperties);

			foreach (SerializedProperty property in ignoreProperties)
			{
				if (property == null || string.IsNullOrEmpty(property.propertyPath))
				{
					continue;
				}

				ignorePropertyNames.Add(property.propertyPath);
			}

			while (iterator.NextVisible(enterChildren))
			{
				enterChildren = false;
				if (ignorePropertyNames.Contains(iterator.name))
				{
					continue;
				}

				root.Add(new PropertyField(iterator));
			}

			hasCreatedDefaultInspector = true;
		}

		protected virtual void CreateGUIBeforeStackTraces(VisualElement root) { }

		protected virtual void GetExcludingProperties(List<SerializedProperty> properties) { }
	}
}