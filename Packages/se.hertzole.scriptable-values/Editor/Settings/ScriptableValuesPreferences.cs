﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Editor
{
	internal sealed class ScriptableValuesPreferences : SettingsProvider
	{
		public static bool CollectStackTraces
		{
			get { return EditorPrefs.GetBool(COLLECT_STACK_TRACES_KEY, true); }
			set
			{
				if (EditorPrefs.GetBool(COLLECT_STACK_TRACES_KEY, true) != value)
				{
					EditorPrefs.SetBool(COLLECT_STACK_TRACES_KEY, value);
					OnCollectStackTracesChanged?.Invoke(value);
				}
			}
		}

		private ScriptableValuesPreferences(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords) { }

		public static event Action<bool> OnCollectStackTracesChanged;

		private const string COLLECT_STACK_TRACES_KEY = "Hertzole.ScriptableValues.CollectStackTraces";

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			VisualElement container = new VisualElement
			{
				style =
				{
					marginLeft = 9,
					marginTop = 1
				}
			};

			Label title = new Label("Scriptable Values Preferences")
			{
				style =
				{
					fontSize = 19,
					unityFontStyleAndWeight = FontStyle.Bold,
					paddingLeft = 1,
					marginBottom = 11
				}
			};

			Toggle toggle = new Toggle("Collect Stack Traces")
			{
				value = CollectStackTraces
			};

			MakeSettingField(toggle);

			toggle.RegisterValueChangedCallback(evt => { CollectStackTraces = evt.newValue; });

			rootElement.Add(container);

			container.Add(title);
			container.Add(toggle);
		}

		[SettingsProvider]
		public static SettingsProvider CreateSettingsProvider()
		{
			return new ScriptableValuesPreferences("Hertzole/Scriptable Values", SettingsScope.User);
		}

		private static void MakeSettingField<T>(BaseField<T> field)
		{
			field.style.marginLeft = 1;
			field.labelElement.style.minWidth = 250;
		}
	}
}