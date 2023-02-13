using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AuroraPunks.ScriptableValues.Editor.Internal
{
	public sealed class TestGenerator : EditorWindow
	{
		private string fullPath;
		private TextField testsPathField;
		private TextField eventsPathField;
		private TextField eventListenersPathField;
		private TextField valuesPathField;
		private TextField valueListenersPathField;

		private const string TESTS_PATH_KEY = "AuroraPunks.ScriptableValues.Editor.TestGenerator.TestsPath";
		private const string EVENTS_PATH_KEY = "AuroraPunks.ScriptableValues.Editor.TestGenerator.EventsPath";
		private const string EVENT_LISTENERS_PATH_KEY = "AuroraPunks.ScriptableValues.Editor.TestGenerator.EventListenersPath";
		private const string VALUES_PATH_KEY = "AuroraPunks.ScriptableValues.Editor.TestGenerator.ValuesPath";
		private const string VALUE_LISTENERS_PATH_KEY = "AuroraPunks.ScriptableValues.Editor.TestGenerator.ValueListenersPath";

		private void CreateGUI()
		{
			ScrollView root = new ScrollView(ScrollViewMode.Vertical)
			{
				style =
				{
					paddingTop = 4
				}
			};

			rootVisualElement.Add(root);

			fullPath = EditorPrefs.GetString(TESTS_PATH_KEY, string.Empty);

			testsPathField = new TextField("Tests Path")
			{
				value = ShortenPath(fullPath)
			};

			Button testPathBrowseButton = new Button(OnClickBrowseTestPath)
			{
				text = "Browse Test Path",
				style =
				{
					marginTop = 8
				}
			};

			// Events

			eventsPathField = new TextField("Events Path")
			{
				value = EditorPrefs.GetString(EVENTS_PATH_KEY, "Events"),
				style =
				{
					marginTop = 16
				}
			};

			// Event listeners

			eventListenersPathField = new TextField("Event Listeners Path")
			{
				value = EditorPrefs.GetString(EVENT_LISTENERS_PATH_KEY, "Event Listeners")
			};

			// Values

			valuesPathField = new TextField("Values Path")
			{
				value = EditorPrefs.GetString(VALUES_PATH_KEY, "Values")
			};

			// Value listeners

			valueListenersPathField = new TextField("Value Listeners Path")
			{
				value = EditorPrefs.GetString(VALUE_LISTENERS_PATH_KEY, "Value Listeners")
			};

			Button generateButton = new Button(OnClickGenerate)
			{
				text = "Generate",
				style =
				{
					marginTop = 16
				}
			};

			root.Add(testsPathField);
			root.Add(testPathBrowseButton);
			root.Add(eventsPathField);
			root.Add(eventListenersPathField);
			root.Add(valuesPathField);
			root.Add(valueListenersPathField);
			root.Add(generateButton);
		}

		[MenuItem("Tools/Test Generator")]
		public static void ShowWindow()
		{
			TestGenerator window = GetWindow<TestGenerator>("Test Generator");
			window.Show();
		}

		private void OnClickBrowseTestPath()
		{
			string path = EditorUtility.OpenFolderPanel("Select Tests Folder", string.IsNullOrEmpty(fullPath) ? "Assets" : fullPath, "");
			if (string.IsNullOrEmpty(path))
			{
				return;
			}

			fullPath = path;
			EditorPrefs.SetString(TESTS_PATH_KEY, path);
			testsPathField.value = ShortenPath(path);
		}

		private void OnClickGenerate()
		{
			EditorPrefs.SetString(TESTS_PATH_KEY, fullPath);
			EditorPrefs.SetString(EVENTS_PATH_KEY, eventsPathField.value);
			EditorPrefs.SetString(EVENT_LISTENERS_PATH_KEY, eventListenersPathField.value);
			EditorPrefs.SetString(VALUES_PATH_KEY, valuesPathField.value);
			EditorPrefs.SetString(VALUE_LISTENERS_PATH_KEY, valueListenersPathField.value);

			TypeCache.TypeCollection values = GetScriptableValues();
			TypeCache.TypeCollection events = GetScriptableEvents();
			TypeCache.TypeCollection allListeners = TypeCache.GetTypesDerivedFrom<ScriptableListenerBase>();
			IEnumerable<Type> valueListeners = allListeners.Where(x => x.BaseType != null && x.BaseType.GenericTypeArguments.Length > 0 && x.BaseType.Name.Contains("ScriptableValueListener"));
			IEnumerable<Type> eventListeners = allListeners.Where(x => x.BaseType != null && x.BaseType.GenericTypeArguments.Length > 0 && x.BaseType.Name.Contains("ScriptableEventListener"));
			GenerateValues(values, Path.Combine(fullPath, valuesPathField.value));
			GenerateEvents(events, Path.Combine(fullPath, eventsPathField.value));
			GenerateValueListeners(valueListeners, values, Path.Combine(fullPath, valueListenersPathField.value));
			GenerateEventListeners(eventListeners, events, Path.Combine(fullPath, eventListenersPathField.value));
			
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}

		private static void GenerateValues(TypeCache.TypeCollection types, string path)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Type type in types)
			{
				if(type.BaseType == null || type.BaseType.GenericTypeArguments.Length == 0 || !TryGetValidType(type.BaseType.GenericTypeArguments[0], out string typeName))
				{
					continue;
				}

				sb.Clear();
				
				sb.AppendLine("namespace AuroraPunks.ScriptableValues.Tests.Values");
				sb.AppendLine("{");
				sb.AppendLine($"\tpublic class {type.Name}ValueTests : ScriptableValueTest<{type.Name}, {typeName}>");
				sb.AppendLine("\t{");
				sb.AppendLine($"\t\tprotected override {typeName} MakeDifferentValue({typeName} value)");
				sb.AppendLine("\t\t{");
				if (typeName == "bool")
				{
					sb.AppendLine("\t\t\treturn !value;");
				}
				else if (typeName == "string")
				{
					sb.AppendLine("\t\t\treturn value + \"1\";");
				}
				else
				{
					sb.AppendLine($"\t\t\treturn ({typeName}) (value - 1);");
				}
				sb.AppendLine("\t\t}");
				sb.AppendLine("\t}");
				sb.AppendLine("}");

				CreateTestFile(Path.Combine(path, $"{type.Name}ValueTests.cs"), sb.ToString());
			}
		}
		
		private static void GenerateEvents(TypeCache.TypeCollection types, string path)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Type type in types)
			{
				if(type.BaseType == null || type.BaseType.GenericTypeArguments.Length == 0 || !TryGetValidType(type.BaseType.GenericTypeArguments[0], out string typeName))
				{
					continue;
				}

				sb.Clear();
				
				sb.AppendLine("namespace AuroraPunks.ScriptableValues.Tests.Events");
				sb.AppendLine("{");
				sb.AppendLine($"\tpublic class {type.Name}Tests : ScriptableEventTest<{type.Name}, {typeName}> {{ }}");
				sb.AppendLine("}");

				CreateTestFile(Path.Combine(path, $"{type.Name}Tests.cs"), sb.ToString());
			}
		}

		private static void GenerateValueListeners(IEnumerable<Type> types, TypeCache.TypeCollection values, string path)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Type type in types)
			{
				if(type.BaseType == null || type.BaseType.GenericTypeArguments.Length == 0 || !TryGetValidType(type.BaseType.GenericTypeArguments[0], out string typeName))
				{
					continue;
				}

				sb.Clear();
				
				sb.AppendLine("namespace AuroraPunks.ScriptableValues.Tests.ValueListeners");
				sb.AppendLine("{");
				sb.AppendLine($"\tpublic class {type.Name}ValueTests : ValueListenerTest<{type.Name}, {GetMatchingType(values, type.BaseType.GenericTypeArguments[0]).Name}, {typeName}> {{ }}");
				sb.AppendLine("}");

				CreateTestFile(Path.Combine(path, $"{type.Name}ValueTests.cs"), sb.ToString());
			}
		}
		
		private static void GenerateEventListeners(IEnumerable<Type> types, TypeCache.TypeCollection events, string path)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Type type in types)
			{
				if(type.BaseType == null || type.BaseType.GenericTypeArguments.Length == 0 || !TryGetValidType(type.BaseType.GenericTypeArguments[0], out string typeName))
				{
					continue;
				}

				sb.Clear();
				
				sb.AppendLine("namespace AuroraPunks.ScriptableValues.Tests.EventListeners");
				sb.AppendLine("{");
				sb.AppendLine($"\tpublic class {type.Name}Tests : GenericEventListenerTest<{type.Name}, {GetMatchingType(events, type.BaseType.GenericTypeArguments[0]).Name}, {typeName}> {{ }}");
				sb.AppendLine("}");

				CreateTestFile(Path.Combine(path, $"{type.Name}Tests.cs"), sb.ToString());
			}
		}

		private static void CreateTestFile(string path, string content)
		{
			if (!Directory.Exists(Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path)!);
			}
			
			File.WriteAllText(path, content);
		}

		private static string ShortenPath(string path)
		{
			return path.Replace(Application.dataPath.Substring(0, Application.dataPath.Length - 6), string.Empty);
		}
		
		private static Type GetMatchingType(TypeCache.TypeCollection types, Type type)
		{
			return types.FirstOrDefault(x => x.BaseType != null && x.BaseType.GenericTypeArguments.Length > 0 && x.BaseType.GenericTypeArguments[0] == type);
		}

		private static bool TryGetValidType(Type type, out string typeName)
		{
			if (type == typeof(byte))
			{
				typeName = "byte";
				return true;
			}
			
			if (type == typeof(sbyte))
			{
				typeName = "sbyte";
				return true;
			}
			
			if (type == typeof(short))
			{
				typeName = "short";
				return true;
			}
			
			if (type == typeof(ushort))
			{
				typeName = "ushort";
				return true;
			}
			
			if (type == typeof(int))
			{
				typeName = "int";
				return true;
			}
			
			if (type == typeof(uint))
			{
				typeName = "uint";
				return true;
			}
			
			if (type == typeof(long))
			{
				typeName = "long";
				return true;
			}
			
			if (type == typeof(ulong))
			{
				typeName = "ulong";
				return true;
			}
			
			if (type == typeof(float))
			{
				typeName = "float";
				return true;
			}
			
			if (type == typeof(double))
			{
				typeName = "double";
				return true;
			}
			
			if (type == typeof(decimal))
			{
				typeName = "decimal";
				return true;
			}
			
			if (type == typeof(bool))
			{
				typeName = "bool";
				return true;
			}
			
			if (type == typeof(string))
			{
				typeName = "string";
				return true;
			}

			typeName = null;
			return false;
		}

		private static TypeCache.TypeCollection GetScriptableValues()
		{
			return TypeCache.GetTypesDerivedFrom<ScriptableValue>();
		}
		
		private static TypeCache.TypeCollection GetScriptableEvents()
		{
			return TypeCache.GetTypesDerivedFrom<ScriptableEvent>();
		}
	}
}