using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AuroraPunks.ScriptableValues.Debugging;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace AuroraPunks.ScriptableValues.Editor
{
	public sealed class StackTraceElement<T> : VisualElement, IDisposable where T : IStackTraceProvider
	{
		private readonly Color borderColorDark = new Color32(26, 26, 26, 255);
		
		private readonly ListView stackTraceList;
		private readonly ListView detailsList;

		private readonly Label emptyDetailsLabel;

		private T target;

		private readonly List<StackFrame> stackFrames = new List<StackFrame>();

		public StackTraceElement(T target)
		{
			StackTraceElement<T> root = this;

			style.borderBottomColor = borderColorDark;
			style.borderBottomWidth = 1;
			style.borderTopColor = borderColorDark;
			style.borderTopWidth = 1;
			style.borderLeftColor = borderColorDark;
			style.borderLeftWidth = 1;
			style.borderRightColor = borderColorDark;
			style.borderRightWidth = 1;
			style.flexGrow = 1;

			// Create the toolbar.
			Toolbar toolbar = new Toolbar();
			toolbar.Add(CreateToolbarLabel("Stack Traces"));

			// Create the splitter.
			TwoPaneSplitView splitter = new TwoPaneSplitView(1, 120, TwoPaneSplitViewOrientation.Vertical);

			// Create the container for the stack traces.
			VisualElement stackTraceContainer = new VisualElement
			{
				style =
				{
					minHeight = 100
				}
			};

			// Create the details container for the stack traces.
			VisualElement detailsContainer = new VisualElement
			{
				style =
				{
					minHeight = 100
				}
			};

			// Create the stack trace list.
			stackTraceList = new ListView
			{
				makeItem = MakeStackTraceItem,
				bindItem = BindStackTraceItem,
				fixedItemHeight = 20,
				reorderable = false,
				showAlternatingRowBackgrounds = AlternatingRowBackground.All,
				itemsSource = target.Invocations,
				showBorder = false
			};

			stackTraceList.onSelectionChange += OnStackTraceListSelectionChanged;
			stackTraceList.onItemsChosen += OnStackTraceListItemsChosen;

			VisualElement stackTraceDetails = new VisualElement
			{
				style =
				{
					borderTopColor = borderColorDark,
					borderTopWidth = 1
				}
			};

			emptyDetailsLabel = new Label("Select a stack trace to view details.")
			{
				style =
				{
					justifyContent = Justify.Center,
					unityTextAlign = TextAnchor.MiddleCenter
				}
			};

			detailsList = new ListView
			{
				makeItem = MakeDetailsItem,
				bindItem = BindDetailsItem,
				fixedItemHeight = 36,
				reorderable = false,
				showAlternatingRowBackgrounds = AlternatingRowBackground.All
			};

			detailsList.onItemsChosen += OnDetailsListItemsChosen;

			stackTraceDetails.Add(detailsList);

			stackTraceContainer.Add(stackTraceList);
			detailsContainer.Add(detailsList);

			// Add toolbar.
			root.Add(toolbar);
			// Add splitter.
			root.Add(splitter);

			// Add containers to splitter.
			splitter.Add(stackTraceContainer);
			splitter.Add(detailsContainer);

			// Add empty details label.
			detailsContainer.Add(emptyDetailsLabel);

			this.target = target;
			target.OnStackTraceAdded += OnStackTraceAdded;
		}

		private void OnStackTraceAdded()
		{
			stackTraceList.RefreshItems();
			if (stackTraceList.selectedIndex >= 0)
			{
				stackTraceList.selectedIndex++;
			}
		}

		private static void OnStackTraceListItemsChosen(IEnumerable<object> obj)
		{
			foreach (object o in obj)
			{
				if (!(o is StackTraceEntry entry))
				{
					continue;
				}

				StackFrame frame = entry.trace.GetSecondOrBestFrame();
				string fileName = frame.GetFileName();
				if (string.IsNullOrEmpty(fileName))
				{
					return;
				}

				string path = ToLocalPath(fileName);
				Object targetScript = AssetDatabase.LoadMainAssetAtPath(path);
				AssetDatabase.OpenAsset(targetScript, frame.GetFileLineNumber(), frame.GetFileColumnNumber());
			}
		}

		private static VisualElement MakeStackTraceItem()
		{
			return new Label
			{
				style =
				{
					paddingLeft = 4,
					unityTextAlign = TextAnchor.MiddleLeft
				}
			};
		}

		private static VisualElement MakeDetailsItem()
		{
			VisualElement root = new VisualElement
			{
				style =
				{
					justifyContent = Justify.Center
				}
			};

			VisualElement nameLabel = new Label
			{
				name = "name",
				style =
				{
					paddingLeft = 4,
					unityTextAlign = TextAnchor.MiddleLeft
				}
			};

			VisualElement pathLabel = new Label
			{
				name = "path",
				style =
				{
					paddingLeft = 4,
					unityTextAlign = TextAnchor.MiddleLeft
				}
			};

			root.Add(nameLabel);
			root.Add(pathLabel);

			return root;
		}

		private void BindStackTraceItem(VisualElement element, int index)
		{
			if (element is Label label)
			{
				StackTraceEntry entry = target.Invocations[index];
				label.text = $"[{entry.hour:00}:{entry.minute:00}:{entry.second:00}] {entry.trace.GetSecondOrBestFrame().GetMethod().Name}";
			}
		}

		private void BindDetailsItem(VisualElement element, int index)
		{
			Label nameLabel = element.Q<Label>("name");
			Label pathLabel = element.Q<Label>("path");

			StackFrame entry = stackFrames[index];
			nameLabel.text = entry.GetMethod().Name;
			pathLabel.text = entry.GetFileName();

			string fileName = entry.GetFileName();

			bool hasFileName = !string.IsNullOrEmpty(fileName);

			pathLabel.text = hasFileName ? $"{ToLocalPath(fileName)}:{entry.GetFileLineNumber()}" : "No file";
			pathLabel.style.unityFontStyleAndWeight = hasFileName ? FontStyle.Normal : FontStyle.Italic;
			pathLabel.style.color = hasFileName ? new StyleColor(new Color(0.298f, 0.494f, 1f, 1f)) : new StyleColor(StyleKeyword.Null);
		}

		private static void OnDetailsListItemsChosen(IEnumerable<object> obj)
		{
			foreach (object o in obj)
			{
				if (!(o is StackFrame frame))
				{
					continue;
				}

				string fileName = frame.GetFileName();
				if (string.IsNullOrEmpty(fileName))
				{
					return;
				}

				string path = ToLocalPath(fileName);
				Object targetScript = AssetDatabase.LoadMainAssetAtPath(path);
				AssetDatabase.OpenAsset(targetScript, frame.GetFileLineNumber(), frame.GetFileColumnNumber());
			}
		}

		private void OnStackTraceListSelectionChanged(IEnumerable<object> obj)
		{
			stackFrames.Clear();
			bool show = true;

			foreach (object o in obj)
			{
				if (!(o is StackTraceEntry stackTrace))
				{
					continue;
				}

				int count = stackTrace.trace.FrameCount;
				if (count == 0)
				{
					StackFrame frame = stackTrace.trace.GetFrame(0);
					stackFrames.Add(frame);
				}
				else
				{
					for (int i = 1; i < stackTrace.trace.FrameCount; i++)
					{
						StackFrame frame = stackTrace.trace.GetFrame(i);
						stackFrames.Add(frame);
					}
				}

				detailsList.itemsSource = stackFrames;
				detailsList.RefreshItems();
				show = false;
			}

			emptyDetailsLabel.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
		}

		private static Label CreateToolbarLabel(string text)
		{
			return new Label(text)
			{
				style =
				{
					marginLeft = 4,
					unityTextAlign = TextAnchor.MiddleLeft
				}
			};
		}

		private static string ToLocalPath(string path)
		{
			return Path.GetFullPath(path).Substring(Application.dataPath.Length - 6).Replace('\\', '/');
		}

		public void Dispose()
		{
			if (target != null)
			{
				target.OnStackTraceAdded -= OnStackTraceAdded;
			}
		}
	}
}