#nullable enable

using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	public class TextElementField : BaseField<string>
	{
		private readonly Label textElement;

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

		public TextElementField(string label) : this(label, string.Empty) { }

		public TextElementField(string label, string value) : base(label, null)
		{
			textElement = new Label(value)
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