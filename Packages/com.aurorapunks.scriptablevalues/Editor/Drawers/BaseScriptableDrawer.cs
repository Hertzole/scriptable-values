using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues
{
	public abstract class BaseScriptableDrawer : PropertyDrawer
	{
		private Label valueLabel;
		private ObjectField field;

		private Type[] types;
		private string noneString;

		private static readonly string[] valueFieldLabelClasses =
		{
			"unity-text-element",
			"unity-label",
			"unity-object-field-display__label"
		};

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			bool isGenericType = fieldInfo.FieldType.IsGenericType;

			if (isGenericType)
			{
				types = GetTypes();

				noneString = $"None ({ObjectNames.NicifyVariableName(GetNameWithoutGenericArity(fieldInfo.FieldType))}<{string.Join(", ", types.Select(x => x.Name))}>)";
			}

			field = new ObjectField(property.displayName)
			{
				tooltip = property.tooltip,
				objectType = fieldInfo.FieldType
			};

			field.AddToClassList(BaseField<object>.alignedFieldUssClassName);

			if (isGenericType)
			{
				field.RegisterCallback<GeometryChangedEvent>(OnValueFieldGeometryChanged);
				field.RegisterCallback<ChangeEvent<string>>(OnValueStringChanged);
			}

			field.BindProperty(property);

			return field;
		}

		private void OnValueFieldGeometryChanged(GeometryChangedEvent evt)
		{
			valueLabel ??= field.Q<Label>(classes: valueFieldLabelClasses);

			UpdateValueFieldLabel();
		}

		private void OnValueStringChanged(ChangeEvent<string> evt)
		{
			// Because Unity is weird, we need to update the label here too if a string changes. 
			// This fixes the issue where the label would not update when the component was first added.
			if (evt.newValue.StartsWith("None") && valueLabel != null)
			{
				valueLabel.text = noneString;
			}
		}

		private void UpdateValueFieldLabel()
		{
			if (valueLabel == null || field.value != null)
			{
				return;
			}

			valueLabel.text = noneString;
		}

		private static string GetNameWithoutGenericArity(Type t)
		{
			string name = t.Name;
			int index = name.IndexOf('`');
			return index == -1 ? name : name.Substring(0, index);
		}

		protected abstract Type[] GetTypes();
	}
}