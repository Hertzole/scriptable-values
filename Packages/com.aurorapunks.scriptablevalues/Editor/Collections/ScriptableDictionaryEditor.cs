using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableDictionary<,>), true)]
	public class ScriptableDictionaryEditor : RuntimeScriptableObjectEditor
	{
		private bool previousIsValid = true;

		
		private IList keysValue;
		private IList valuesValue;
		
		private ListView dictionaryListView;
		private HelpBox errorBox;

		private PropertyField isReadOnlyField;
		private PropertyField setEqualityCheckElement;
		private PropertyField clearOnStartElement;

		private ScriptableDictionary dictionary;

		private SerializedProperty isReadOnly;
		private SerializedProperty setEqualityCheck;
		private SerializedProperty clearOnStart;
		private SerializedProperty keys;
		private SerializedProperty values;

		protected override string StackTracesLabel { get { return "Dictionary Change Stack Traces"; } }

		protected override void OnEnable()
		{
			base.OnEnable();

			dictionary = (ScriptableDictionary) target;

			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		protected override void GatherProperties()
		{
			isReadOnly = serializedObject.FindProperty(nameof(isReadOnly));
			setEqualityCheck = serializedObject.FindProperty(nameof(setEqualityCheck));
			clearOnStart = serializedObject.FindProperty(nameof(clearOnStart));
			keys = serializedObject.FindProperty(nameof(keys));
			values = serializedObject.FindProperty(nameof(values));

			if (keys == null || values == null)
			{
				FieldInfo keysFieldInfo = target.GetType().GetField(nameof(ScriptableDictionary<object, object>.keys), BindingFlags.NonPublic | BindingFlags.Default | BindingFlags.Public | BindingFlags.Instance);
				keysValue = keysFieldInfo!.GetValue(target) as IList;

				FieldInfo valuesFieldInfo = target.GetType().GetField(nameof(ScriptableDictionary<object, object>.values), BindingFlags.NonPublic | BindingFlags.Default | BindingFlags.Public | BindingFlags.Instance);
				valuesValue = valuesFieldInfo!.GetValue(target) as IList;

				FixLists(keysValue, valuesValue);
			}
		}

		protected override void CreateGUIBeforeStackTraces(VisualElement root)
		{
			errorBox = new HelpBox("There are keys with the same value. All keys must be unique!", HelpBoxMessageType.Error);

			isReadOnlyField = new PropertyField(isReadOnly);
			setEqualityCheckElement = new PropertyField(setEqualityCheck);
			clearOnStartElement = new PropertyField(clearOnStart);

			isReadOnlyField.BindProperty(serializedObject);
			setEqualityCheckElement.Bind(serializedObject);
			clearOnStartElement.Bind(serializedObject);

			if (keys != null && values != null)
			{
				dictionaryListView = new ListView
				{
					showFoldoutHeader = true,
					showAddRemoveFooter = true,
					showBorder = true,
					headerTitle = "Dictionary",
					reorderMode = ListViewReorderMode.Animated,
					reorderable = true,
					makeItem = MakeDictionaryItem,
					bindItem = BindDictionaryItem,
					unbindItem = UnbindDictionaryItem,
					virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight
				};

				dictionaryListView.itemsAdded += OnItemsAdded;
				dictionaryListView.itemsRemoved += OnItemsRemoved;

				dictionaryListView.BindProperty(keys);
			}
			else
			{
				dictionaryListView = new ListView(keysValue, -1, MakeDictionaryItem, BindDictionaryItem)
				{
					headerTitle = "Dictionary",
					showBorder = true,
					showAddRemoveFooter = false,
					showFoldoutHeader = true,
					reorderable = false,
					showBoundCollectionSize = false,
					unbindItem = UnbindDictionaryItem,
					virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight
				};
			}

			isReadOnlyField.RegisterValueChangeCallback(evt => UpdateVisibility());

			UpdateErrorBox();
			UpdateVisibility();
			UpdateEnabledState();

			root.Add(errorBox);
			root.Add(isReadOnlyField);
			root.Add(setEqualityCheckElement);
			root.Add(clearOnStartElement);

			CreateDefaultInspectorGUI(root);

			root.Add(dictionaryListView);
		}

		private void OnPlayModeStateChanged(PlayModeStateChange obj)
		{
			UpdateEnabledState();
		}

		private void UpdateErrorBox()
		{
			bool isValid = dictionary.IsValid();

			errorBox.SetVisibility(!isValid);

			if (previousIsValid != isValid)
			{
				dictionaryListView.RefreshItems();
				previousIsValid = isValid;
			}
		}

		private void UpdateVisibility()
		{
			bool show = !isReadOnly.boolValue;
			setEqualityCheckElement.SetVisibility(show);
			clearOnStartElement.SetVisibility(show);
		}

		private void UpdateEnabledState()
		{
			bool enabled = !isReadOnly.boolValue || !EditorApplication.isPlayingOrWillChangePlaymode;
			isReadOnlyField.SetEnabled(enabled);
			dictionaryListView.SetEnabled(enabled);
		}

		private void OnItemsAdded(IEnumerable<int> newItems)
		{
			foreach (int newItem in newItems)
			{
				values.InsertArrayElementAtIndex(newItem);
				values.serializedObject.ApplyModifiedProperties();
			}

			UpdateErrorBox();
		}

		private void OnItemsRemoved(IEnumerable<int> removedItems)
		{
			foreach (int removedItem in removedItems)
			{
				int oldCount = values.arraySize;

				values.DeleteArrayElementAtIndex(removedItem);

				// We may need to delete twice because Unity thought it was a good idea to sometimes not remove the element
				// from the array. ಠ╭╮ಠ
				if (values.arraySize == oldCount)
				{
					values.DeleteArrayElementAtIndex(removedItem);
				}

				values.serializedObject.ApplyModifiedProperties();
			}

			UpdateErrorBox();
		}

		private void BindDictionaryItem(VisualElement element, int index)
		{
			PropertyField keyPropertyField = element.Q<PropertyField>("key-element-property");
			PropertyField valuePropertyField = element.Q<PropertyField>("value-element-property");
			DynamicValueField keyDynamicField = element.Q<DynamicValueField>("key-element-dynamic");
			DynamicValueField valueDynamicField = element.Q<DynamicValueField>("value-element-dynamic");

			if (keys != null && values != null)
			{
				if (keys.arraySize <= index || values.arraySize <= index)
				{
					return;
				}

				keyPropertyField.style.display = DisplayStyle.Flex;
				valuePropertyField.style.display = DisplayStyle.Flex;
				keyDynamicField.style.display = DisplayStyle.None;
				valueDynamicField.style.display = DisplayStyle.None;

				SerializedProperty key = keys.GetArrayElementAtIndex(index);
				SerializedProperty value = values.GetArrayElementAtIndex(index);

				keyPropertyField.label = string.Empty;
				keyPropertyField.BindProperty(key);
				valuePropertyField.label = string.Empty;
				valuePropertyField.BindProperty(value);

				VisualElement errorElement = element.Q<VisualElement>("error-element");
				errorElement.SetVisibility(!dictionary.IsIndexValid(index));

				keyPropertyField.RegisterCallback<SerializedPropertyChangeEvent, (int, VisualElement)>(OnKeyChanged, (index, errorElement));
			}
			else
			{
				keyPropertyField.style.display = DisplayStyle.None;
				valuePropertyField.style.display = DisplayStyle.None;
				keyDynamicField.style.display = DisplayStyle.Flex;
				valueDynamicField.style.display = DisplayStyle.Flex;

				object key = keysValue[index];
				object value = valuesValue[index];

				keyDynamicField.label = string.Empty;
				keyDynamicField.value = key;
				valueDynamicField.label = string.Empty;
				valueDynamicField.value = value;

				VisualElement errorElement = element.Q<VisualElement>("error-element");
				errorElement.SetVisibility(!dictionary.IsIndexValid(index));
			}
		}

		private void UnbindDictionaryItem(VisualElement element, int index)
		{
			PropertyField keyElement = element.Q<PropertyField>("key-element");
			keyElement.UnregisterCallback<SerializedPropertyChangeEvent, (int, VisualElement)>(OnKeyChanged);
		}

		private void OnKeyChanged(SerializedPropertyChangeEvent evt, (int index, VisualElement errorElement) context)
		{
			context.errorElement.SetVisibility(!dictionary.IsIndexValid(context.index));
			UpdateErrorBox();
		}

		private VisualElement MakeDictionaryItem()
		{
			VisualElement root = new VisualElement
			{
				style =
				{
					flexDirection = FlexDirection.Row,
				}
			};

			// If there's any types that can't be serialized, add a slight margin to the list items.
			// For some reason, if we don't do this it will show a small scrollbar in the list.
			if (keys == null || values == null)
			{
				root.style.marginRight = 2;
			}

			VisualElement errorElement = new VisualElement
			{
				name = "error-element",
				style =
				{
					width = 20,
					height = 20,
					marginRight = 2
				}
			};

			errorElement.AddToClassList("unity-help-box__icon--error");

			VisualElement keyHolder = new VisualElement
			{
				style =
				{
					width = new StyleLength(Length.Percent(50)),
					marginRight = 2
				}
			};

			VisualElement valueHolder = new VisualElement
			{
				style =
				{
					width = new StyleLength(Length.Percent(50)),
					marginLeft = 2
				}
			};

			PropertyField keyElement = new PropertyField
			{
				name = "key-element-property"
			};

			DynamicValueField dynamicKeyField = new DynamicValueField
			{
				name = "key-element-dynamic"
			};

			PropertyField valueElement = new PropertyField
			{
				name = "value-element-property"
			};

			DynamicValueField dynamicValueField = new DynamicValueField
			{
				name = "value-element-dynamic"
			};

			root.Add(errorElement);
			root.Add(keyHolder);
			root.Add(valueHolder);

			keyHolder.Add(keyElement);
			keyHolder.Add(dynamicKeyField);
			valueHolder.Add(valueElement);
			valueHolder.Add(dynamicValueField);

			return root;
		}

		private static void FixLists(IList list1, IList list2)
		{
			if (list1.Count > list2.Count)
			{
				// Remove extra items from list1
				for (int i = list1.Count - 1; i >= list2.Count; i--)
				{
					list1.RemoveAt(i);
				}
			}
			else if (list2.Count > list1.Count)
			{
				// Remove extra items from list2
				for (int i = list2.Count - 1; i >= list1.Count; i--)
				{
					list2.RemoveAt(i);
				}
			}
		}

		protected override void GetExcludingProperties(List<SerializedProperty> properties)
		{
			properties.Add(isReadOnly);
			if (keys != null)
			{
				properties.Add(keys);
			}

			if (values != null)
			{
				properties.Add(values);
			}

			properties.Add(setEqualityCheck);
			properties.Add(clearOnStart);
		}
	}
}