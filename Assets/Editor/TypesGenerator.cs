using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor.Internal
{
	public sealed class TypesGenerator : EditorWindow
	{
		private string fullPath;
		private TextField testsPathField;
		private TextField eventsPathField;
		private TextField eventListenersPathField;
		private TextField valuesPathField;
		private TextField valueListenersPathField;
		private TextField addressablesPathField;
		private TextField valueReferencesPathField;

		private static readonly Type[] typesToGenerate =
		{
			typeof(byte),
			typeof(sbyte),
			typeof(short),
			typeof(ushort),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(bool),
			typeof(string),
			typeof(char),
			typeof(Color),
			typeof(Color32),
			typeof(Vector2),
			typeof(Vector3),
			typeof(Vector4),
			typeof(Vector2Int),
			typeof(Vector3Int),
			typeof(Quaternion),
			typeof(Rect),
			typeof(RectInt),
			typeof(Bounds),
			typeof(BoundsInt)
		};

		private const string BASE_KEY = "Hertzole.ScriptableValues.Editor.TypesGenerator.";
		private const string PATH_KEY = BASE_KEY + "Path";
		private const string EVENTS_PATH_KEY = BASE_KEY + "EventsPath";
		private const string EVENT_LISTENERS_PATH_KEY = BASE_KEY + "EventListenersPath";
		private const string VALUES_PATH_KEY = BASE_KEY + "ValuesPath";
		private const string VALUE_LISTENERS_PATH_KEY = BASE_KEY + "ValueListenersPath";
		private const string ADDRESSABLES_PATH_KEY = BASE_KEY + "AddressablesPath";
		private const string VALUE_REFERENCES_PATH = BASE_KEY + "ValueReferencesPath";

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

			fullPath = EditorPrefs.GetString(PATH_KEY, string.Empty);

			testsPathField = new TextField("Path")
			{
				value = ShortenPath(fullPath)
			};

			Button testPathBrowseButton = new Button(OnClickBrowseTestPath)
			{
				text = "Browse Path",
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
			
			// Value references
			
			valueReferencesPathField = new TextField("Value References Path")
			{
				value = EditorPrefs.GetString(VALUE_REFERENCES_PATH, "Value References")
			};
			
			// Addressables
			
			addressablesPathField = new TextField("Addressables Path")
			{
				value = EditorPrefs.GetString(ADDRESSABLES_PATH_KEY, "Addressables")
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
			root.Add(valueReferencesPathField);
			root.Add(addressablesPathField);
			root.Add(generateButton);
		}

		[MenuItem("Tools/Types Generator")]
		public static void ShowWindow()
		{
			TypesGenerator window = GetWindow<TypesGenerator>("Types Generator");
			window.Show();
		}

		private void OnClickBrowseTestPath()
		{
			string path = EditorUtility.OpenFolderPanel("Select Types Folder", string.IsNullOrEmpty(fullPath) ? "Assets" : fullPath, "");
			if (string.IsNullOrEmpty(path))
			{
				return;
			}

			fullPath = path;
			EditorPrefs.SetString(PATH_KEY, path);
			testsPathField.value = ShortenPath(path);
		}

		private void OnClickGenerate()
		{
			EditorPrefs.SetString(PATH_KEY, fullPath);
			EditorPrefs.SetString(EVENTS_PATH_KEY, eventsPathField.value);
			EditorPrefs.SetString(EVENT_LISTENERS_PATH_KEY, eventListenersPathField.value);
			EditorPrefs.SetString(VALUES_PATH_KEY, valuesPathField.value);
			EditorPrefs.SetString(VALUE_LISTENERS_PATH_KEY, valueListenersPathField.value);
			EditorPrefs.SetString(ADDRESSABLES_PATH_KEY, addressablesPathField.value);
			EditorPrefs.SetString(VALUE_REFERENCES_PATH, valueReferencesPathField.value);

			GenerateValues(Path.Combine(fullPath, valuesPathField.value));
			GenerateEvents(Path.Combine(fullPath, eventsPathField.value));
			GenerateValueListeners(Path.Combine(fullPath, valueListenersPathField.value));
			GenerateValueReferences(Path.Combine(fullPath, valueReferencesPathField.value));
			GenerateEventListeners(Path.Combine(fullPath, eventListenersPathField.value));
			GenerateAddressables(Path.Combine(fullPath, addressablesPathField.value));

			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}

		private static void GenerateValues(string path)
		{
			StringBuilder sb = new StringBuilder();
			int index = 0;
			foreach (Type type in typesToGenerate)
			{
				string namespaceName = null;
				string name = GetBetterName(type);

				if (!TryGetValidType(type, out string typeName))
				{
					typeName = type.Name;
					namespaceName = type.Namespace;
				}

				sb.Clear();

				if (!string.IsNullOrEmpty(namespaceName) && namespaceName != "UnityEngine")
				{
					sb.AppendLine($"using {namespaceName};");
				}

				sb.AppendLine("using UnityEngine;");
				sb.AppendLine();

				sb.AppendLine("namespace Hertzole.ScriptableValues");
				sb.AppendLine("{");
				sb.AppendLine("\t/// <summary>");
				sb.AppendLine($"\t///     <see cref=\"ScriptableValue{{T}}\" /> with a <see cref=\"{typeName}\"/> value.");
				sb.AppendLine("\t/// </summary>");
				sb.AppendLine("#if UNITY_EDITOR");
				sb.AppendLine($"\t[CreateAssetMenu(fileName = \"New Scriptable {ObjectNames.NicifyVariableName(name)}\", menuName = \"Hertzole/Scriptable Values/Values/{name} Value\", order = ORDER + {index})]");
				sb.AppendLine("#endif");
				sb.AppendLine($"\tpublic sealed class Scriptable{name} : ScriptableValue<{typeName}> {{ }}");
				sb.AppendLine("}");

				CreateTestFile(Path.Combine(path, $"Scriptable{name}.cs"), sb.ToString());
				index++;
			}
		}

		private static void GenerateEvents(string path)
		{
			StringBuilder sb = new StringBuilder();
			int index = 1;
			foreach (Type type in typesToGenerate)
			{
				string namespaceName = null;
				string name = GetBetterName(type);

				if (!TryGetValidType(type, out string typeName))
				{
					typeName = type.Name;
					namespaceName = type.Namespace;
				}

				sb.Clear();

				if (!string.IsNullOrEmpty(namespaceName) && namespaceName != "UnityEngine")
				{
					sb.AppendLine($"using {namespaceName};");
				}

				sb.AppendLine("using UnityEngine;");
				sb.AppendLine();

				sb.AppendLine("namespace Hertzole.ScriptableValues");
				sb.AppendLine("{");
				sb.AppendLine("\t/// <summary>");
				sb.AppendLine($"\t///     <see cref=\"ScriptableEvent{{T}}\" /> with a <see cref=\"{typeName}\"/> argument.");
				sb.AppendLine("\t/// </summary>");
				sb.AppendLine("#if UNITY_EDITOR");
				sb.AppendLine($"\t[CreateAssetMenu(fileName = \"New Scriptable {ObjectNames.NicifyVariableName(name)} Event\", menuName = \"Hertzole/Scriptable Values/Events/{name} Event\", order = ORDER + {index})]");
				sb.AppendLine("#endif");
				sb.AppendLine($"\tpublic sealed class Scriptable{name}Event : ScriptableEvent<{typeName}> {{ }}");
				sb.AppendLine("}");

				CreateTestFile(Path.Combine(path, $"Scriptable{name}Event.cs"), sb.ToString());
				index++;
			}
		}

		private static void GenerateValueListeners(string path)
		{
			StringBuilder sb = new StringBuilder();
			int index = 1000;
			foreach (Type type in typesToGenerate)
			{
				string namespaceName = null;
				string name = GetBetterName(type);

				if (!TryGetValidType(type, out string typeName))
				{
					typeName = type.Name;
					namespaceName = type.Namespace;
				}

				sb.Clear();

				if (!string.IsNullOrEmpty(namespaceName) && namespaceName != "UnityEngine")
				{
					sb.AppendLine($"using {namespaceName};");
				}

				sb.AppendLine("using UnityEngine;");
				sb.AppendLine();

				sb.AppendLine("namespace Hertzole.ScriptableValues");
				sb.AppendLine("{");
				sb.AppendLine("\t/// <summary>");
				sb.AppendLine("\t///     A <see cref=\"ScriptableValueListener{TValue}\" /> that listens to a <see cref=\"ScriptableValue{TValue}\" /> with a");
				sb.AppendLine($"\t///     type of <see cref=\"{typeName}\" /> and invokes an <see cref=\"UnityEngine.Events.UnityEvent\" /> when the value changes.");
				sb.AppendLine("\t/// </summary>");
				sb.AppendLine("#if UNITY_EDITOR");
				sb.AppendLine($"\t[AddComponentMenu(\"Scriptable Values/Listeners/Values/Scriptable {name} Listener\", {index})]");
				sb.AppendLine("#endif");
				sb.AppendLine($"\tpublic sealed class Scriptable{name}Listener : ScriptableValueListener<{typeName}> {{ }}");
				sb.AppendLine("}");

				CreateTestFile(Path.Combine(path, $"Scriptable{name}Listener.cs"), sb.ToString());
				index++;
			}
		}

		private static void GenerateValueReferences(string path)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Type type in typesToGenerate)
			{
				string namespaceName = null;
				string name = GetBetterName(type);

				if (!TryGetValidType(type, out string typeName))
				{
					typeName = type.Name;
					namespaceName = type.Namespace;
				}

				sb.Clear();

				bool hasNamespace = false;

				if (!string.IsNullOrEmpty(namespaceName) && namespaceName != "UnityEngine")
				{
					sb.AppendLine($"using {namespaceName};");
					hasNamespace = true;
				}

				if (!string.IsNullOrEmpty(type.Namespace))
				{
					sb.AppendLine($"using {type.Namespace};");
					hasNamespace = true;
				}

				if (hasNamespace)
				{
					sb.AppendLine();
				}

				sb.AppendLine("namespace Hertzole.ScriptableValues");
				sb.AppendLine("{");
				sb.AppendLine("\t/// <summary>");
				sb.AppendLine($"\t///     A <see cref=\"ValueReference{{TValue}}\" /> with a type of <see cref=\"{typeName}\" />");
				sb.AppendLine($"\t///     that allows you to reference a <see cref=\"ScriptableValue{{{type.Name}}}\" /> or a constant value.");
				sb.AppendLine("\t/// </summary>");
				sb.AppendLine("\t[System.Serializable]");
				sb.AppendLine($"\tpublic sealed class {name}Reference : ValueReference<{typeName}> {{ }}");
				sb.AppendLine("}");

				CreateTestFile(Path.Combine(path, $"{name}Reference.cs"), sb.ToString());
			}
		}

		private static void GenerateEventListeners(string path)
		{
			StringBuilder sb = new StringBuilder();
			int index = 1100;
			foreach (Type type in typesToGenerate)
			{
				string namespaceName = null;
				string name = GetBetterName(type);

				if (!TryGetValidType(type, out string typeName))
				{
					typeName = type.Name;
					namespaceName = type.Namespace;
				}

				sb.Clear();

				if (!string.IsNullOrEmpty(namespaceName) && namespaceName != "UnityEngine")
				{
					sb.AppendLine($"using {namespaceName};");
				}

				sb.AppendLine("using UnityEngine;");
				sb.AppendLine();

				sb.AppendLine("namespace Hertzole.ScriptableValues");
				sb.AppendLine("{");
				sb.AppendLine("\t/// <summary>");
				sb.AppendLine("\t///     A <see cref=\"ScriptableEventListener{TValue}\" /> that listens to a <see cref=\"ScriptableEvent{TValue}\" /> with a");
				sb.AppendLine($"\t///     type of <see cref=\"{typeName}\" /> and invokes an <see cref=\"UnityEngine.Events.UnityEvent\" /> when the event is invoked.");
				sb.AppendLine("\t/// </summary>");
				sb.AppendLine("#if UNITY_EDITOR");
				sb.AppendLine($"\t[AddComponentMenu(\"Scriptable Values/Listeners/Values/Scriptable {name} Event Listener\", {index})]");
				sb.AppendLine("#endif");
				sb.AppendLine($"\tpublic sealed class Scriptable{name}EventListener : ScriptableEventListener<{typeName}> {{ }}");
				sb.AppendLine("}");

				CreateTestFile(Path.Combine(path, $"Scriptable{name}EventListener.cs"), sb.ToString());
				index++;
			}
		}
		
		private static void GenerateAddressables(string path)
		{
			StringBuilder sb = new StringBuilder();
			
			// Values
			foreach (Type type in typesToGenerate)
			{
				string name = GetBetterName(type);
				GenerateAddressablesType(sb, $"Scriptable{name}");
				CreateTestFile(Path.Combine(path, "Values", $"AssetReferenceScriptable{name}.cs"), sb.ToString());
			}

			// Events
			
			GenerateAddressablesType(sb, "ScriptableEvent");
			CreateTestFile(Path.Combine(path, "Events", "AssetReferenceScriptableEvent.cs"), sb.ToString());
			
			foreach (Type type in typesToGenerate)
			{
				string name = GetBetterName(type);
				GenerateAddressablesType(sb, $"Scriptable{name}Event");
				CreateTestFile(Path.Combine(path, "Events", $"AssetReferenceScriptable{name}Event.cs"), sb.ToString());
			}
		}

		private static void GenerateAddressablesType(StringBuilder sb, string typeName)
		{
			sb.Clear();

			sb.AppendLine("#if SCRIPTABLE_VALUES_ADDRESSABLES");
			sb.AppendLine("using System;");
			sb.AppendLine("using UnityEngine.AddressableAssets;\n");
			sb.AppendLine("namespace Hertzole.ScriptableValues");
			sb.AppendLine("{");
			sb.AppendLine("\t/// <summary>");
			sb.AppendLine($"\t///     <see cref=\"{typeName}\" /> only asset reference.");
			sb.AppendLine("\t/// </summary>");
			sb.AppendLine("\t[Serializable]");
			sb.AppendLine($"\tpublic sealed class AssetReference{typeName} : AssetReferenceT<{typeName}>");
			sb.AppendLine("\t{");
			sb.AppendLine("\t\t/// <summary>");
			sb.AppendLine($"\t\t///     Constructs a new reference to a <see cref=\"AssetReference{typeName}\" />.");
			sb.AppendLine("\t\t/// </summary>");
			sb.AppendLine("\t\t/// <param name=\"guid\">The object guid.</param>");
			sb.AppendLine("#if UNITY_EDITOR || UNITY_INCLUDE_TESTS");
			sb.AppendLine("\t\t[UnityEngine.TestTools.ExcludeFromCoverage]");
			sb.AppendLine("#endif");
			sb.AppendLine($"\t\tpublic AssetReference{typeName}(string guid) : base(guid) {{ }}");
			sb.AppendLine("\t}");
			sb.AppendLine("}");
			sb.AppendLine("#endif");
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

			if (type == typeof(char))
			{
				typeName = "char";
				return true;
			}

			typeName = null;
			return false;
		}

		private static string GetBetterName(Type type)
		{
			if (type == typeof(byte))
			{
				return "Byte";
			}

			if (type == typeof(sbyte))
			{
				return "SByte";
			}

			if (type == typeof(short))
			{
				return "Short";
			}

			if (type == typeof(ushort))
			{
				return "UShort";
			}

			if (type == typeof(int))
			{
				return "Int";
			}

			if (type == typeof(uint))
			{
				return "UInt";
			}

			if (type == typeof(long))
			{
				return "Long";
			}

			if (type == typeof(ulong))
			{
				return "ULong";
			}

			if (type == typeof(float))
			{
				return "Float";
			}

			if (type == typeof(double))
			{
				return "Double";
			}

			if (type == typeof(decimal))
			{
				return "Decimal";
			}

			if (type == typeof(bool))
			{
				return "Bool";
			}

			if (type == typeof(string))
			{
				return "String";
			}

			return type.Name;
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