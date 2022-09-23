using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AuroraPunks.ScriptableValues
{
	public abstract class RuntimeScriptableObject : ScriptableObject
	{
		private void OnEnable()
		{
#if UNITY_EDITOR
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#else
            ResetValues();
#endif
		}

		public virtual void ResetValues() { }

#if UNITY_EDITOR
		private void OnDisable()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		private void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			switch (state)
			{
				case PlayModeStateChange.ExitingEditMode:
					ResetValues();
					break;
				case PlayModeStateChange.EnteredEditMode:
					OnExitPlayMode();
					break;
			}
		}

		protected virtual void OnExitPlayMode() { }
#endif
	}
}