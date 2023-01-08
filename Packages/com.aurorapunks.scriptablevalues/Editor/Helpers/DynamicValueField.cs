using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace AuroraPunks.ScriptableValues.Editor
{
	internal sealed class DynamicValueField : BaseField<object>
	{
		private readonly VisualElement container;

		private ObjectField objectField;
		private TextField textField;
		private Label labelField;

		public override object value
		{
			get { return base.value; }
			set
			{
				if (EqualityComparer<object>.Default.Equals(base.value, value))
				{
					return;
				}

				base.value = value;

				UpdateValueField();
			}
		}

		public DynamicValueField() : base(null, null)
		{
			container = this.Q<VisualElement>(className: "unity-base-field__input");
		}

		private void UpdateValueField()
		{
			ValueType type = GetValueType(base.value);

			UpdateFields(type);

			switch (type)
			{
				case ValueType.Unknown:
				case ValueType.Null:
					ShowUnknownOrLabel();
					break;
				case ValueType.Object:
					ShowObjectField();
					break;
				case ValueType.Text:
					ShowTextField();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void ShowObjectField()
		{
			if (objectField == null)
			{
				objectField = new ObjectField();
				container.Add(objectField);
				objectField.SetEnabled(false);
			}

			objectField.objectType = base.value.GetType();
			objectField.value = base.value as Object;
		}

		private void ShowTextField()
		{
			if (textField == null)
			{
				textField = new TextField();
				container.Add(textField);
				textField.SetEnabled(false);
			}

			textField.value = base.value as string;
		}

		private void ShowUnknownOrLabel()
		{
			if (labelField == null)
			{
				labelField = new Label
				{
					style =
					{
						marginTop = 2
					}
				};

				container.Add(labelField);
			}

			labelField.text = base.value?.ToString() ?? "null";
		}

		private void UpdateFields(ValueType type)
		{
			if (objectField != null)
			{
				objectField.style.display = type == ValueType.Object ? DisplayStyle.Flex : DisplayStyle.None;
			}

			if (textField != null)
			{
				textField.style.display = type == ValueType.Text ? DisplayStyle.Flex : DisplayStyle.None;
			}

			if (labelField != null)
			{
				labelField.style.display = type == ValueType.Unknown || type == ValueType.Null ? DisplayStyle.Flex : DisplayStyle.None;
			}
		}

		private static ValueType GetValueType(object value)
		{
			if (value == null)
			{
				return ValueType.Unknown;
			}

			if (value is string)
			{
				return ValueType.Text;
			}

			if (value is Object)
			{
				return ValueType.Object;
			}

			return ValueType.Unknown;
		}

		private enum ValueType
		{
			Unknown,
			Object,
			Text,
			Null
		}
	}
}