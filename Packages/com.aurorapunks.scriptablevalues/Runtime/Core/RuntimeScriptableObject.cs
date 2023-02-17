using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.TestTools;
#endif

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     Base class for scriptable objects that have values that should be resettable.
	/// </summary>
	public abstract partial class RuntimeScriptableObject : ScriptableObject
	{
		/// <summary>
		///     Subscribes to play mode state changed to listen for play mode events.
		///     <para>When the game is built, it calls the <see cref="ResetValues" /> method.</para>
		/// </summary>
		private void OnEnable()
		{
#if UNITY_EDITOR
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#else
            OnStart();
#endif
		}

#if UNITY_EDITOR
		/// <summary>
		///     Resets the values of the scriptable object.
		///     <para>Automatically called when the game is started, both in the editor and in the build.</para>
		/// </summary>
		[Obsolete("Use OnStart instead for initialization logic that should be called when the game starts.", true)]
		[ExcludeFromCoverage]
		public virtual void ResetValues() { }
#endif

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
			switch (state)
			{
				case PlayModeStateChange.ExitingEditMode:
					OnStart();
					break;
				case PlayModeStateChange.EnteredEditMode:
					OnExitPlayMode();
					break;
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