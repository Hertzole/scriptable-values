using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     Base class for scriptable objects that have values that should be resettable.
	/// </summary>
	public abstract partial class RuntimeScriptableObject : ScriptableObject
	{
		/// <summary>
		///     Subscribes to play mode state changed to listen for play mode events.
		///     <para>When the game is built, it calls the <see cref="OnStart" /> method.</para>
		/// </summary>
		private void OnEnable()
		{
#if UNITY_EDITOR
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#else
            OnStart();
#endif
		}

		/// <summary>
		///     Called when the game starts.
		/// </summary>
		protected virtual void OnStart() { }

#if UNITY_EDITOR
		/// <summary>
		///     Unsubscribes from play mode state changed to stop listening for play mode events.
		/// </summary>
		private void OnDisable()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		/// <summary>
		///     Called when the play mode state changes.
		/// </summary>
		/// <param name="state">The new play mode state.</param>
		private void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.ExitingEditMode)
			{
				OnStart();
			}
			else if (state == PlayModeStateChange.EnteredEditMode)
			{
				OnExitPlayMode();
			}
		}

		/// <summary>
		///     Internal method for calling OnExitPlayMode in tests.
		/// </summary>
		internal void Test_ExitPlayMode()
		{
			OnExitPlayMode();
		}

		/// <summary>
		///     Internal method for calling OnStart in tests.
		/// </summary>
		internal void Test_OnStart()
		{
			OnStart();
		}

		/// <summary>
		///     Called when the game exits play mode.
		/// </summary>
		protected virtual void OnExitPlayMode() { }
#endif
	}
}