using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableDictionary<,>), true)]
	public class ScriptableDictionaryEditor : RuntimeScriptableObjectEditor
	{
		private bool previousIsValid = true;
		private HelpBox errorBox;
		private ListView dictionaryListView;

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
			if (keys.arraySize <= index || values.arraySize <= index)
			{
				return;
			}
			
			SerializedProperty key = keys.GetArrayElementAtIndex(index);
			SerializedProperty value = values.GetArrayElementAtIndex(index);

			PropertyField keyElement = element.Q<PropertyField>("key-element");
			keyElement.label = string.Empty;
			keyElement.BindProperty(key);
			PropertyField valueElement = element.Q<PropertyField>("value-element");
			valueElement.label = string.Empty;
			valueElement.BindProperty(value);

			VisualElement errorElement = element.Q<VisualElement>("error-element");
			errorElement.SetVisibility(!dictionary.IsIndexValid(index));

			keyElement.RegisterCallback<SerializedPropertyChangeEvent, (int, VisualElement)>(OnKeyChanged, (index, errorElement));
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

		private static VisualElement MakeDictionaryItem()
		{
			VisualElement root = new VisualElement
			{
				style =
				{
					flexDirection = FlexDirection.Row
				}
			};

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
				name = "key-element"
			};

			PropertyField valueElement = new PropertyField
			{
				name = "value-element"
			};

			root.Add(errorElement);
			root.Add(keyHolder);
			root.Add(valueHolder);

			keyHolder.Add(keyElement);
			valueHolder.Add(valueElement);

			return root;
		}

		protected override void GetExcludingProperties(List<SerializedProperty> properties)
		{
			properties.Add(isReadOnly);
			properties.Add(keys);
			properties.Add(values);
			properties.Add(setEqualityCheck);
			properties.Add(clearOnStart);
		}
	}
}