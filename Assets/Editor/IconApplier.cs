using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hertzole.ScriptableValues.Editor.Internal
{
	public static class IconApplier
	{
		[MenuItem("Tools/Apply Icons")]
		public static void ApplyIcons()
		{
			string[] assets = AssetDatabase.FindAssets("t:MonoScript");

			List<string> scriptsToApply = new List<string>();

			foreach (string asset in assets)
			{
				string path = AssetDatabase.GUIDToAssetPath(asset);

				if (!path.StartsWith("Packages/se.hertzole.scriptable-values/Runtime"))
				{
					continue;
				}

				Type c = AssetDatabase.LoadAssetAtPath<MonoScript>(path).GetClass();

				if (c == null)
				{
					continue;
				}

				scriptsToApply.Add(path);
			}

			if (scriptsToApply.Count == 0)
			{
				return;
			}

			AssetDatabase.StartAssetEditing();

			foreach (string path in scriptsToApply)
			{
				Type c = AssetDatabase.LoadAssetAtPath<MonoScript>(path).GetClass();
				EditorUtility.DisplayProgressBar("Applying icons", "Applying icons to " + c.Name, scriptsToApply.IndexOf(path) / (float) scriptsToApply.Count);

				string[] icons = AssetDatabase.FindAssets($"t:Texture2D {c.Name}Icon");

				if (icons.Length == 0)
				{
					Debug.LogWarning($"No icons for {c.Name} not found.");
					continue;
				}

				MonoImporter monoImporter = AssetImporter.GetAtPath(path) as MonoImporter;

				if (monoImporter == null)
				{
					Debug.LogWarning($"Importer for {c.Name} not found.");
					continue;
				}

				Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(icons[0]));

				if (icon == null)
				{
					Debug.LogWarning($"Icon for {c.Name} not found.");
					continue;
				}

				monoImporter.SetIcon(icon);

				AssetDatabase.ImportAsset(path);
			}

			EditorUtility.ClearProgressBar();

			AssetDatabase.StopAssetEditing();

			AssetDatabase.Refresh();
		}
	}
}