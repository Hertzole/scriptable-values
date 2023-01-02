using System.Reflection;
using AuroraPunks.ScriptableValues.Debugging;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor
{
	[CustomEditor(typeof(ScriptablePool<>), true)]
	public class ScriptablePoolEditor : UnityEditor.Editor
	{
		private Label countAllLabel;
		private Label countActiveLabel;
		private Label countInactiveLabel;

		private PropertyInfo countAllProperty;
		private PropertyInfo countActiveProperty;
		private PropertyInfo countInactiveProperty;
		private SerializedProperty collectStackTraces;

		private StackTraceElement stackTraces;
		private VisualElement contentViewport;

		protected virtual void OnEnable()
		{
			collectStackTraces = serializedObject.FindProperty(nameof(collectStackTraces));

			countAllProperty = target.GetType().GetProperty(nameof(ScriptablePool<object>.CountAll));
			countActiveProperty = target.GetType().GetProperty(nameof(ScriptablePool<object>.CountActive));
			countInactiveProperty = target.GetType().GetProperty(nameof(ScriptablePool<object>.CountInactive));

			((IStackTraceProvider) target).OnStackTraceAdded += UpdateCounts;
		}

		private void OnDisable()
		{
			((IStackTraceProvider) target).OnStackTraceAdded -= UpdateCounts;
		}

		private void UpdateCounts()
		{
			if (countAllLabel != null)
			{
				countAllLabel.text = countAllProperty.GetValue(target).ToString();
			}

			if (countActiveLabel != null)
			{
				countActiveLabel.text = countActiveProperty.GetValue(target).ToString();
			}

			if (countInactiveLabel != null)
			{
				countInactiveLabel.text = countInactiveProperty.GetValue(target).ToString();
			}
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new EntireInspectorElement();

			stackTraces = new StackTraceElement((IStackTraceProvider) target, collectStackTraces, "Invocation Stack Traces")
			{
				style =
				{
					marginTop = 4
				}
			};

			VisualElement countAll = CreateLabelField("Count All", out countAllLabel);
			VisualElement countActive = CreateLabelField("Count Active", out countActiveLabel);
			VisualElement countInactive = CreateLabelField("Count Inactive", out countInactiveLabel);

			root.Add(countAll);
			root.Add(countActive);
			root.Add(countInactive);

			root.Add(stackTraces);

			UpdateCounts();

			return root;
		}

		private static VisualElement CreateLabelField(string labelText, out Label labelValue)
		{
			PropertyField root = new PropertyField();
			labelValue = new Label();
			TextElementField textField = new TextElementField(labelText, labelValue);
			textField.AddToClassList("unity-base-field__aligned");
			root.Add(textField);

			return root;
		}
	}
}