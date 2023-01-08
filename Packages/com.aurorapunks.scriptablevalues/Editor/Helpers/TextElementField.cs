using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	public class TextElementField : BaseField<string>
	{
		private Label textElement;

		public override string value
		{
			get { return base.value; }
			set
			{
				if (EqualityComparer<string>.Default.Equals(this.value, value))
				{
					return;
				}

				base.value = value;
				textElement.text = value;
			}
		}

		public TextElementField() : base(string.Empty, null)
		{
			Setup(string.Empty, string.Empty);
		}

		public TextElementField(string label) : base(label, null)
		{
			Setup(label, string.Empty);
		}

		public TextElementField(string label, string value) : base(label, null)
		{
			Setup(label, value);
		}

		private void Setup(string labelText, string text)
		{
			label = labelText;
			textElement = new Label(text)
			{
				style =
				{
					paddingTop = 2
				}
			};

			textElement.AddToClassList(inputUssClassName);

			VisualElement input = this.Q<VisualElement>(className: "unity-base-field__input");

			input.Add(textElement);
		}
	}
}