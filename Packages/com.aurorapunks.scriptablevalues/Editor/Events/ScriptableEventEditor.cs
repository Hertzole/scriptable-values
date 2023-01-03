using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableEvent), true)]
	public class ScriptableEventEditor : RuntimeScriptableObjectEditor
	{
		private ScriptableEvent scriptableEvent;
		private SerializedProperty onInvoked;

		protected override void GatherProperties()
		{
			scriptableEvent = (ScriptableEvent) target;

			onInvoked = serializedObject.FindProperty(nameof(onInvoked));
		}

		protected override void CreateGUIBeforeStackTraces(VisualElement root)
		{
			PropertyField onInvokedField = new PropertyField(onInvoked);
			onInvokedField.Bind(serializedObject);

			VisualElement invokeElement = CreateInvokeButton();

			if (invokeElement != null)
			{
				invokeElement.style.marginBottom = 8;
				root.Add(invokeElement);
			}

			root.Add(onInvokedField);
		}

		protected virtual VisualElement CreateInvokeButton()
		{
			return new Button(OnClickInvoke)
			{
				text = "Invoke"
			};
		}

		private void OnClickInvoke()
		{
			scriptableEvent.Invoke(this);
		}

		protected override void GetExcludingProperties(List<SerializedProperty> properties)
		{
			properties.Add(onInvoked);
		}
	}
}