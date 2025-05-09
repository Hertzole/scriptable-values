﻿#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableList<>), true)]
	public class ScriptableListEditor : RuntimeScriptableObjectEditor
	{
		private FieldInfo listFieldInfo = null!;
		private IList listValue = null!;
		private ListView? dynamicListView;
		private PropertyField setEqualityCheckField = null!;
		private PropertyField isReadOnlyField = null!;
		private PropertyField clearOnStartField = null!;
		private PropertyField? listField;
		private SerializedProperty setEqualityCheck = null!;
		private SerializedProperty isReadOnly = null!;
		private SerializedProperty clearOnStart = null!;
		private SerializedProperty? list;

		protected override string StackTracesLabel
		{
			get { return "List Change Stack Traces"; }
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		protected override void GatherProperties()
		{
			setEqualityCheck = serializedObject.MustFindProperty(nameof(setEqualityCheck));
			isReadOnly = serializedObject.MustFindProperty(nameof(isReadOnly));
			clearOnStart = serializedObject.MustFindProperty(nameof(clearOnStart));
			list = serializedObject.MustFindProperty(nameof(list));

			if (list == null)
			{
				listFieldInfo = target.GetType().GetField(nameof(ScriptableList<object>.list),
					BindingFlags.NonPublic | BindingFlags.Default | BindingFlags.Public | BindingFlags.Instance)!;

				listValue = (listFieldInfo.GetValue(target) as IList)!;

				Assert.IsNotNull(listValue);
			}
		}

		protected override void OnStackTraceAdded()
		{
			base.OnStackTraceAdded();

			dynamicListView?.RefreshItems();
		}

		protected override void CreateGUIBeforeStackTraces(VisualElement root)
		{
			setEqualityCheckField = new PropertyField(setEqualityCheck);
			isReadOnlyField = new PropertyField(isReadOnly);
			clearOnStartField = new PropertyField(clearOnStart);

			if (list != null)
			{
				listField = new PropertyField(list);
				listField.Bind(serializedObject);
			}
			else
			{
				dynamicListView = new ListView(listValue, EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, MakeListItem,
					BindListItem)
				{
					showBorder = true,
					showFoldoutHeader = true,
					showAddRemoveFooter = false,
					showBoundCollectionSize = false,
					reorderable = false,
					headerTitle = "List"
				};
			}

			setEqualityCheckField.Bind(serializedObject);
			isReadOnlyField.Bind(serializedObject);
			clearOnStartField.Bind(serializedObject);

			isReadOnlyField.RegisterValueChangeCallback(static (evt, args) => { args.UpdateVisibility(evt.changedProperty.boolValue); }, this);

			UpdateVisibility(isReadOnly.boolValue);
			UpdateEnabledState();

			root.Add(isReadOnlyField);
			root.Add(setEqualityCheckField);
			root.Add(clearOnStartField);

			CreateDefaultInspectorGUI(root);

			if (list != null)
			{
				root.Add(listField);
			}
			else
			{
				root.Add(dynamicListView);
			}
		}

		private static VisualElement MakeListItem()
		{
			DynamicValueField field = new DynamicValueField
			{
				// If there's any types that can't be serialized, add a slight margin to the list items.
				// For some reason, if we don't do this it will show a small scrollbar in the list.
				style =
				{
					marginRight = 2
				}
			};

			field.AddToClassList(TextElementField.alignedFieldUssClassName);
			return field;
		}

		private void BindListItem(VisualElement element, int index)
		{
			if (!(element is DynamicValueField textField))
			{
				return;
			}

			textField.label = $"Element {index}";
			textField.value = listValue[index];
		}

		private void UpdateVisibility(bool readOnly)
		{
			setEqualityCheckField.SetVisibility(!readOnly);
			clearOnStartField.SetVisibility(!readOnly);
		}

		private void UpdateEnabledState()
		{
			// Find the isReadOnly property again because it may have been disposed.
			// The property is always disposed when exiting play mode.
			isReadOnly = serializedObject.FindProperty(nameof(isReadOnly));

			bool enabled = !isReadOnly.boolValue || !EditorApplication.isPlayingOrWillChangePlaymode;
			isReadOnlyField.SetEnabled(enabled);
			listField?.SetEnabled(enabled);
		}

		private void OnPlayModeStateChanged(PlayModeStateChange obj)
		{
			UpdateEnabledState();
		}

		protected override void GetExcludingProperties(List<SerializedProperty> properties)
		{
			properties.Add(setEqualityCheck);
			properties.Add(isReadOnly);
			properties.Add(clearOnStart);
			// If the list type can't be serialized, the list property will be null.
			if (list != null)
			{
				properties.Add(list);
			}
		}
	}
}