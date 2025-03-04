#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hertzole.ScriptableValues.Debugging
{
	internal static class UserSettings
	{
		// Used to keep track of temporary objects, like objects used in testing.
		private static readonly Dictionary<int, bool> temporaryCollectStackTraces = new Dictionary<int, bool>();

		private static UserSettingsData? data;

		private static readonly string dataPath =
			Path.GetFullPath(Application.dataPath + "/../UserSettings/Packages/se.hertzole.scriptable-values/UserSettings.json");

		private static UserSettingsData Data
		{
			get
			{
				if (data.HasValue)
				{
					return data.Value;
				}

				MakeSurePathExists();

				if (!File.Exists(dataPath))
				{
					return new UserSettingsData
					{
						collectStackTraces = false,
						collectStackTracesData = new List<CollectStackTracesData>()
					};
				}

				string json = File.ReadAllText(dataPath, Encoding.UTF8);
				data = JsonUtility.FromJson<UserSettingsData>(json);
				return data.Value;
			}
		}

		public static bool CollectStackTraces
		{
			get { return Data.collectStackTraces; }
			set
			{
				if (Data.collectStackTraces == value)
				{
					return;
				}

				UserSettingsData localData = Data;
				localData.collectStackTraces = value;
				data = localData;

				SaveData();
			}
		}

		public static bool GetCollectStackTraces(Object obj)
		{
			string guid = GetKey(obj);

			// If the object is not an asset, we use the temporary dictionary.
			if (string.IsNullOrEmpty(guid))
			{
				return temporaryCollectStackTraces.GetValueOrDefault(obj.GetInstanceID(), false);
			}

			if (Data.TryGetCollectStackTraces(guid, out bool value))
			{
				return value;
			}

			return false;
		}

		public static void SetCollectStackTraces(Object obj, bool value)
		{
			string guid = GetKey(obj);

			// If the object is not an asset, we use the temporary dictionary.
			if (string.IsNullOrEmpty(guid))
			{
				temporaryCollectStackTraces[obj.GetInstanceID()] = value;
			}

			Data.SetCollectStackTraces(guid, value);
			SaveData();
		}

		private static string GetKey(Object obj)
		{
			return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj));
		}

		private static void SaveData()
		{
			MakeSurePathExists();

			string json = JsonUtility.ToJson(Data, true);
			File.WriteAllText(dataPath, json, Encoding.UTF8);
		}

		private static void MakeSurePathExists()
		{
			string directoryPath = Path.GetDirectoryName(dataPath);

			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath!);
			}
		}

		[Serializable]
		public struct UserSettingsData
		{
			public bool collectStackTraces;
			public List<CollectStackTracesData> collectStackTracesData;

			public bool TryGetCollectStackTraces(string key, out bool value)
			{
				for (int i = 0; i < collectStackTracesData.Count; i++)
				{
					if (collectStackTracesData[i].key == key)
					{
						value = collectStackTracesData[i].value;
						return true;
					}
				}

				value = false;
				return false;
			}

			public void SetCollectStackTraces(string key, bool value)
			{
				for (int i = 0; i < collectStackTracesData.Count; i++)
				{
					if (collectStackTracesData[i].key == key)
					{
						collectStackTracesData[i] = new CollectStackTracesData { key = key, value = value };
						return;
					}
				}

				collectStackTracesData.Add(new CollectStackTracesData { key = key, value = value });
			}
		}

		[Serializable]
		public struct CollectStackTracesData
		{
			public string key;
			public bool value;
		}
	}
}
#endif