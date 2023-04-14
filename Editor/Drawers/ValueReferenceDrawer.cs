using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues
{
	[CustomPropertyDrawer(typeof(ValueReference<>), true)]
	public sealed class ValueReferenceDrawer : PropertyDrawer
	{
		private static GUIContent icon;

		private SerializedProperty typeProperty;
		private PropertyField referenceField;
		private PropertyField constantField;
		private PropertyField addressableField;

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			icon ??= EditorGUIUtility.IconContent("AnimationWrapModeMenu");

			VisualElement root = new VisualElement
			{
				style =
				{
					flexDirection = FlexDirection.Row
				}
			};

			VisualElement valueContainer = new VisualElement
			{
				style =
				{
					flexGrow = 1
				}
			};

			typeProperty = property.FindPropertyRelative("valueType");

			VisualElement menuButton = new VisualElement
			{
				style =
				{
					backgroundImage = new StyleBackground(icon.image as Texture2D),
					width = 16,
					height = 16,
					marginLeft = 6,
					alignSelf = Align.Center
				}
			};

			menuButton.RegisterCallback<PointerDownEvent, SerializedProperty>((_, prop) => { CreateMenu(prop, true); }, typeProperty);

			menuButton.Bind(property.serializedObject);
			
			string label =
#if UNITY_2022_2_OR_NEWER
				preferredLabel;
#else
				property.displayName;
#endif

			constantField = new PropertyField(property.FindPropertyRelative("constantValue"), label);
			referenceField = new PropertyField(property.FindPropertyRelative("referenceValue"), label);

			valueContainer.Add(constantField);
			valueContainer.Add(referenceField);
#if SCRIPTABLE_VALUES_ADDRESSABLES
			addressableField = new PropertyField(property.FindPropertyRelative("addressableReference"), label);
			valueContainer.Add(addressableField);
#endif

			root.Add(valueContainer);
			root.Add(menuButton);

			RefreshFields();

			return root;
		}

		private void RefreshFields()
		{
			constantField.style.display = typeProperty.enumValueIndex == (int) ValueReferenceType.Constant ? DisplayStyle.Flex : DisplayStyle.None;
			referenceField.style.display = typeProperty.enumValueIndex == (int) ValueReferenceType.Reference ? DisplayStyle.Flex : DisplayStyle.None;
#if SCRIPTABLE_VALUES_ADDRESSABLES
			addressableField.style.display = typeProperty.enumValueIndex == (int) ValueReferenceType.Addressable ? DisplayStyle.Flex : DisplayStyle.None;
#endif
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			icon ??= EditorGUIUtility.IconContent("AnimationWrapModeMenu");

			typeProperty = property.FindPropertyRelative("valueType");

			ValueReferenceType type = (ValueReferenceType) typeProperty.enumValueIndex;

			Rect rect = new Rect(position.x, position.y, position.width - 22, position.height);

			switch (type)
			{
				case ValueReferenceType.Constant:
					EditorGUI.PropertyField(rect, property.FindPropertyRelative("constantValue"), label, true);
					break;
				case ValueReferenceType.Reference:
					EditorGUI.PropertyField(rect, property.FindPropertyRelative("referenceValue"), label, true);
					break;
#if SCRIPTABLE_VALUES_ADDRESSABLES
				case ValueReferenceType.Addressable:
					EditorGUI.PropertyField(rect, property.FindPropertyRelative("addressableReference"), label, true);
					break;
#endif
				default:
					throw new ArgumentOutOfRangeException();
			}

			if (GUI.Button(new Rect(position.x + position.width - 18, position.y + 2, 16, 16), icon.image, EditorStyles.iconButton))
			{
				CreateMenu(typeProperty, false);
			}
		}

		private void CreateMenu(SerializedProperty prop, bool refreshFields)
		{
			GenericMenu menu = new GenericMenu();
			menu.AddItem(new GUIContent("Use Constant"), (ValueReferenceType) prop.enumValueIndex == ValueReferenceType.Constant, () =>
			{
				prop.enumValueIndex = (int) ValueReferenceType.Constant;
				prop.serializedObject.ApplyModifiedProperties();
				if (refreshFields)
				{
					RefreshFields();
				}
			});

			menu.AddItem(new GUIContent("Use Reference"), (ValueReferenceType) prop.enumValueIndex == ValueReferenceType.Reference, () =>
			{
				prop.enumValueIndex = (int) ValueReferenceType.Reference;
				prop.serializedObject.ApplyModifiedProperties();
				if (refreshFields)
				{
					RefreshFields();
				}
			});

#if SCRIPTABLE_VALUES_ADDRESSABLES
			menu.AddItem(new GUIContent("Use Addressable"), (ValueReferenceType) prop.enumValueIndex == ValueReferenceType.Addressable, () =>
			{
				prop.enumValueIndex = (int) ValueReferenceType.Addressable;
				prop.serializedObject.ApplyModifiedProperties();
				if (refreshFields)
				{
					RefreshFields();
				}
			});
#endif

			menu.ShowAsContext();
		}
	}
}