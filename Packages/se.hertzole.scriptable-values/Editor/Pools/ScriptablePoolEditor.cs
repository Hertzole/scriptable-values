﻿#nullable enable

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptablePool<>), true)]
	public class ScriptablePoolEditor : RuntimeScriptableObjectEditor
	{
		private PropertyInfo countAllProperty = null!;
		private PropertyInfo countActiveProperty = null!;
		private PropertyInfo countInactiveProperty = null!;

		private TextElementField? countAllLabel;
		private TextElementField? countActiveLabel;
		private TextElementField? countInactiveLabel;

		protected override void GatherProperties()
		{
			Type type = target.GetType();

			countAllProperty = type.GetProperty(nameof(ScriptablePool<object>.CountAll))!;
			countActiveProperty = type.GetProperty(nameof(ScriptablePool<object>.CountActive))!;
			countInactiveProperty = type.GetProperty(nameof(ScriptablePool<object>.CountInactive))!;
		}

		protected override void OnStackTraceAdded()
		{
			UpdateCounts();
		}

		private void UpdateCounts()
		{
			if (countAllLabel != null)
			{
				countAllLabel.value = countAllProperty.GetValue(target).ToString();
			}

			if (countActiveLabel != null)
			{
				countActiveLabel.value = countActiveProperty.GetValue(target).ToString();
			}

			if (countInactiveLabel != null)
			{
				countInactiveLabel.value = countInactiveProperty.GetValue(target).ToString();
			}
		}

		protected override void CreateGUIBeforeStackTraces(VisualElement root)
		{
			countAllLabel = CreateLabelField("Count All");
			countActiveLabel = CreateLabelField("Count Active");
			countInactiveLabel = CreateLabelField("Count Inactive");

			root.Add(countAllLabel);
			root.Add(countActiveLabel);
			root.Add(countInactiveLabel);

			UpdateCounts();
		}

		private static TextElementField CreateLabelField(string labelText)
		{
			TextElementField field = new TextElementField(labelText);
			field.AddToClassList(TextElementField.alignedFieldUssClassName);

			return field;
		}
	}
}