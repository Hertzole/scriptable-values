using System.Collections.Generic;
using AuroraPunks.ScriptableValues.Debugging;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptableDictionary<,>), true)]
	public class ScriptableDictionaryEditor : UnityEditor.Editor
	{
		private bool previousIsValid = true;
		
		private PropertyField isReadOnlyField;
		private PropertyField setEqualityCheckElement;
		private PropertyField clearOnStartElement;
		private HelpBox errorBox;
		private ListView dictionaryListView;
		private StackTraceElement stackTraces;

		private ScriptableDictionary dictionary;

		private SerializedProperty isReadOnly;
		private SerializedProperty setEqualityCheck;
		private SerializedProperty clearOnStart;
		private SerializedProperty keys;
		private SerializedProperty values;
		private SerializedProperty collectStackTraces;

		private void OnEnable()
		{
			isReadOnly = serializedObject.FindProperty(nameof(isReadOnly));
			setEqualityCheck = serializedObject.FindProperty(nameof(setEqualityCheck));
			clearOnStart = serializedObject.FindProperty(nameof(clearOnStart));
			keys = serializedObject.FindProperty(nameof(keys));
			values = serializedObject.FindProperty(nameof(values));
			collectStackTraces = serializedObject.FindProperty(nameof(collectStackTraces));

			dictionary = (ScriptableDictionary) target;

			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		private void OnDisable()
		{
			stackTraces?.Dispose();

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		public override VisualElement CreateInspectorGUI()
		{
			EntireInspectorElement root = new EntireInspectorElement();

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

			dictionaryListView.BindProperty(keys);

			stackTraces = new StackTraceElement((IStackTraceProvider) target, collectStackTraces, "Dictionary Change Stack Traces")
			{
				style =
				{
					marginTop = 4
				}
			};

			isReadOnlyField.RegisterValueChangeCallback(evt => UpdateVisibility());

			UpdateErrorBox();
			UpdateVisibility();
			UpdateEnabledState();

			root.Add(errorBox);
			root.Add(isReadOnlyField);
			root.Add(setEqualityCheckElement);
			root.Add(clearOnStartElement);
			root.Add(dictionaryListView);
			root.Add(stackTraces);

			return root;
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

		private void BindDictionaryItem(VisualElement element, int index)
		{
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

		private VisualElement MakeDictionaryItem()
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
	}
}