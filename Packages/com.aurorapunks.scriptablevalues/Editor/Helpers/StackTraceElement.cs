using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AuroraPunks.ScriptableValues.Debugging;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace AuroraPunks.ScriptableValues.Editor
{
	public sealed class StackTraceElement : VisualElement, IDisposable
	{
		private readonly Color borderColorDark = new Color32(26, 26, 26, 255);
		private readonly Color borderColorLight = new Color32(138, 138, 138, 255);

		private readonly ListView stackTraceList;
		private readonly ListView detailsList;

		private readonly Label emptyDetailsLabel;

		private readonly IStackTraceProvider target;

		private readonly List<StackFrame> stackFrames = new List<StackFrame>();

		private Color BorderColor { get { return EditorGUIUtility.isProSkin ? borderColorDark : borderColorLight; } }

		public StackTraceElement(IStackTraceProvider target, string title = "Stack Traces")
		{
			// Set the root as this element.
			StackTraceElement root = this;

			// Setup the border.
			style.borderBottomColor = BorderColor;
			style.borderBottomWidth = 1;
			style.borderTopColor = BorderColor;
			style.borderTopWidth = 1;
			style.borderLeftColor = BorderColor;
			style.borderLeftWidth = 1;
			style.borderRightColor = BorderColor;
			style.borderRightWidth = 1;
			style.flexGrow = 1;

			// Create the toolbar.
			Toolbar toolbar = new Toolbar();

			ToolbarSpacer spacer = new ToolbarSpacer
			{
				style =
				{
					flexGrow = 1
				}
			};

			ToolbarButton clearButton = new ToolbarButton(() =>
			{
				target.Invocations.Clear();
				stackFrames.Clear();
				stackTraceList?.RefreshItems();
				detailsList?.RefreshItems();
				UpdateDetailsVisibility(false);
			})
			{
				text = "Clear",
				style =
				{
					unityTextAlign = TextAnchor.MiddleCenter,
					left = 1 // Setting left to 1 makes it flush with the side.
				}
			};

			toolbar.Add(CreateToolbarLabel(title));
			toolbar.Add(spacer);
			toolbar.Add(clearButton);

			// Create the splitter.
			TwoPaneSplitView splitter = new TwoPaneSplitView(1, 120, TwoPaneSplitViewOrientation.Vertical)
			{
				style =
				{
					minHeight = 300
				}
			};

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

			// Subscribe to the stack trace list events.
			stackTraceList.onSelectionChange += OnStackTraceListSelectionChanged; // When the selection is changed.
			stackTraceList.onItemsChosen += OnStackTraceListItemsChosen; // When an item is double clicked (aka chosen)

			// Create a label to show if no item is selected.
			emptyDetailsLabel = new Label("Select a stack trace to view details.")
			{
				style =
				{
					justifyContent = Justify.Center,
					unityTextAlign = TextAnchor.MiddleCenter,
					marginTop = 10
				}
			};

			// Create the details list.
			detailsList = new ListView
			{
				makeItem = MakeDetailsItem,
				bindItem = BindDetailsItem,
				fixedItemHeight = 36,
				reorderable = false,
				showAlternatingRowBackgrounds = AlternatingRowBackground.All
			};

			// Subscribe to the details list events.
			detailsList.onItemsChosen += OnDetailsListItemsChosen; // When an item is double clicked (aka chosen)

			// Add toolbar.
			root.Add(toolbar);
			// Add splitter.
			root.Add(splitter);

			// Add containers to splitter.
			splitter.Add(stackTraceContainer);
			splitter.Add(detailsContainer);

			// Add the stack trace list to its container.
			stackTraceContainer.Add(stackTraceList);

			// Add empty details label.
			detailsContainer.Add(emptyDetailsLabel);
			// Add the details list.
			detailsContainer.Add(detailsList);

			// Set the target and subscribe to the target events.
			this.target = target;
			target.OnStackTraceAdded += OnStackTraceAdded;
		}

		private void OnStackTraceAdded()
		{
			// Refresh the items at the item source.
			stackTraceList.RefreshItems();
			// If an item is selected, move the selection down as we're adding items above.
			if (stackTraceList.selectedIndex >= 0)
			{
				stackTraceList.selectedIndex++;
			}
		}

		/// <summary>
		///     Binds a stack trace visual element to the given index.
		/// </summary>
		/// <param name="element">The UI element.</param>
		/// <param name="index">The index of the item to bind to.</param>
		private void BindStackTraceItem(VisualElement element, int index)
		{
			// Check for the label.
			if (element is Label label)
			{
				StackTraceEntry entry = target.Invocations[index];
				StackFrame frame = entry.trace.GetFrame(0);
				// Set text to [HH:MM:SS] MethodName (Line:Column)
				label.text = $"[{entry.hour:00}:{entry.minute:00}:{entry.second:00}] {frame.GetMethod().Name} ({frame.GetFileLineNumber()}:{frame.GetFileColumnNumber()})";
			}
		}

		/// <summary>
		///     Binds a details list visual element to the given index.
		/// </summary>
		/// <param name="element">The UI element.</param>
		/// <param name="index">The index of the item to bind to.</param>
		private void BindDetailsItem(VisualElement element, int index)
		{
			// Find the labels.
			Label nameLabel = element.Q<Label>("name");
			Label pathLabel = element.Q<Label>("path");

			StackFrame entry = stackFrames[index];

			// Set the name to the method name.
			nameLabel.text = GetMethodName(entry.GetMethod());

			// Get the file name and check if it exists.
			// It may not exist if the method is native.
			string fileName = entry.GetFileName();
			bool hasFileName = !string.IsNullOrEmpty(fileName);

			// If it has a file name, set the path to the file name.
			// Else just show no file.
			pathLabel.text = hasFileName ? $"{ToLocalPath(fileName)}:{entry.GetFileLineNumber()}" : "No file";
			// If it has a file name, make the text normal.
			// Else make it italic.
			pathLabel.style.unityFontStyleAndWeight = hasFileName ? FontStyle.Normal : FontStyle.Italic;
			// If it has a file name, make the text blue like a hyperlink.
			// Else make it the normal text color.
			pathLabel.style.color = hasFileName ? new StyleColor(new Color(0.298f, 0.494f, 1f, 1f)) : new StyleColor(StyleKeyword.Null);
		}

		/// <summary>
		///     Called when a stack trace item is chosen/double clicked on.
		/// </summary>
		/// <param name="obj">The selected objects.</param>
		private static void OnStackTraceListItemsChosen(IEnumerable<object> obj)
		{
			foreach (object o in obj)
			{
				// Skip if it, for some reason, isn't a stack trace.
				if (!(o is StackTraceEntry entry))
				{
					continue;
				}

				StackFrame frame = entry.trace.GetFrame(0);
				// Open the frame in the code editor.
				OpenStackFrame(frame);
			}
		}

		/// <summary>
		///     Called when the stack trace list selection changes.
		/// </summary>
		/// <param name="obj">The newly selected objects.</param>
		private void OnStackTraceListSelectionChanged(IEnumerable<object> obj)
		{
			// Clear the cached stack frames.
			stackFrames.Clear();
			// Determines if the "Select trace to view details" label should be shown.
			bool showSelectTraceLabel = true;

			foreach (object o in obj)
			{
				// Skip if, for some reason, it isn't a stack trace.
				if (!(o is StackTraceEntry stackTrace))
				{
					continue;
				}

				// Add all the frames.
				StackFrame[] frames = stackTrace.trace.GetFrames();
				if (frames != null)
				{
					stackFrames.AddRange(frames);
				}

				// Update the items source.
				detailsList.itemsSource = stackFrames;
				// We must refresh the items here.
				detailsList.RefreshItems();
				// Don't show the label.
				showSelectTraceLabel = false;
			}

			UpdateDetailsVisibility(!showSelectTraceLabel);
		}

		/// <summary>
		///     Updates the visibility of the details list and the "Select trace to view details" label.
		/// </summary>
		/// <param name="showDetailsList">Toggle to show the list or the label.</param>
		private void UpdateDetailsVisibility(bool showDetailsList)
		{
			// Update the label visibility based on the showSelectTraceLabel variable.
			emptyDetailsLabel.style.display = !showDetailsList ? DisplayStyle.Flex : DisplayStyle.None;
			detailsList.style.display = showDetailsList ? DisplayStyle.Flex : DisplayStyle.None;
		}

		/// <summary>
		///     Called when a stack trace details item is chosen/double clicked on.
		/// </summary>
		/// <param name="obj">The items chosen.</param>
		private static void OnDetailsListItemsChosen(IEnumerable<object> obj)
		{
			foreach (object o in obj)
			{
				if (!(o is StackFrame frame))
				{
					continue;
				}

				// Open the frame in the code editor.
				OpenStackFrame(frame);
			}
		}

		/// <summary>
		///     Creates a new visual element for the stack trace list with just a label.
		/// </summary>
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

		/// <summary>
		///     Creates a new visual element for the details list with two labels, one for name and one for path.
		/// </summary>
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

		/// <summary>
		///     Creates a label for use in the toolbar.
		/// </summary>
		/// <param name="text">The text on the label.</param>
		/// <returns>The new label.</returns>
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

		/// <summary>
		///     Converts a full system path to a local path inside the Assets folder.
		/// </summary>
		/// <param name="path">The system path to convert.</param>
		/// <returns>A path relative to the Unity project.</returns>
		private static string ToLocalPath(string path)
		{
			return Path.GetFullPath(path).Substring(Application.dataPath.Length - 6).Replace('\\', '/');
		}

		/// <summary>
		///     Gets a full method name.
		/// </summary>
		/// <param name="methodInfo">The method to get the name of.</param>
		/// <returns>A pretty name.</returns>
		private static string GetMethodName(MethodBase methodInfo)
		{
			StringBuilder sb = new StringBuilder();

			if (methodInfo.DeclaringType != null)
			{
				if (!string.IsNullOrEmpty(methodInfo.DeclaringType.Namespace))
				{
					sb.Append(methodInfo.DeclaringType.Namespace);
					sb.Append(".");
				}

				sb.Append(ToPrettyName(methodInfo.DeclaringType));
				sb.Append(".");
			}

			sb.Append(methodInfo.Name);

			return sb.ToString();
		}

		/// <summary>
		///     Converts a type name to a pretty name by removing ugly generic type names.
		/// </summary>
		/// <param name="type">The type to get the name from.</param>
		/// <returns>A pretty name.</returns>
		private static string ToPrettyName(Type type)
		{
			StringBuilder sb = new StringBuilder();

			if (type.IsGenericType)
			{
				sb.Append(type.Name.Split('`')[0]);
				sb.Append("<");
				sb.Append(string.Join(", ", type.GetGenericArguments().Select(ToPrettyName)));
				sb.Append(">");
			}
			else
			{
				sb.Append(type.Name);
			}

			return sb.ToString();
		}

		/// <summary>
		///     Tries to open a stack frame in the code editor.
		/// </summary>
		/// <param name="frame">The frame to get the details from.</param>
		private static void OpenStackFrame(StackFrame frame)
		{
			string fileName = frame.GetFileName();
			// If there's no file name, stop because we can't open it.
			if (string.IsNullOrEmpty(fileName))
			{
				return;
			}

			// Convert the path to a Unity path inside the Assets folder and open it in the IDE.
			string path = ToLocalPath(fileName);
			Object targetScript = AssetDatabase.LoadMainAssetAtPath(path);
			AssetDatabase.OpenAsset(targetScript, frame.GetFileLineNumber(), frame.GetFileColumnNumber());
		}

		public void Dispose()
		{
			// Unregister the callback when the window is closed.
			if (target != null)
			{
				target.OnStackTraceAdded -= OnStackTraceAdded;
			}
		}
	}
}