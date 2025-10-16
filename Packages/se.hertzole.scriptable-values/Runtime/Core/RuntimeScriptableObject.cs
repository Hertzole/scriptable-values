using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     Base class for <see cref="ScriptableObject" /> that have values that should be resettable.
    /// </summary>
#if UNITY_EDITOR
    [HelpURL(Documentation.CREATING_CUSTOM_TYPES_URL)]
#endif
    public abstract partial class RuntimeScriptableObject : ScriptableObject, IScriptableValueCallbacks
    {
        /// <summary>
        ///     Subscribes to play mode state changed to listen for play mode events.
        ///     <para>When the game is built, it calls the <see cref="OnStart" /> method.</para>
        /// </summary>
        private void OnEnable()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            // This could be loaded in the editor, but we only want to call OnStart when entering play mode.
            // If it's loaded during play mode, we want to call OnStart right away.
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            OnPreStart();
            OnStart();
        }

        /// <summary>
        ///     Unsubscribes from play mode state changed to stop listening for play mode events.
        ///     <para>When the game is built, it calls the <see cref="OnDisabled" /> method.</para>
        /// </summary>
        private void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

            // This could be loaded in the editor, but we only want to call OnDisabled when entering play mode.
            // If it's loaded during play mode, we want to call OnDisabled right away.
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            OnPreDisabled();
            OnDisabled();
        }

        /// <summary>
        ///     Called when the game starts, before <see cref="OnStart" />.
        ///     <para>
        ///         You should not subscribe to events in this method! Use <see cref="OnStart" /> instead.
        ///         <see cref="OnPreStart" /> should be used for preparing the scriptable object.
        ///     </para>
        /// </summary>
        /// <remarks>If this isn't called when the game starts, it's called once this scriptable object is loaded and instantiated.</remarks>
        protected virtual void OnPreStart() { }

        /// <summary>
        ///     Called when the game starts, after <see cref="OnPreStart" />.
        ///     <para>You should not prepare the object in this method! Use <see cref="OnPreStart" /> instead.</para>
        /// </summary>
        /// <remarks>If this isn't called when the game starts, it's called once this scriptable object is loaded and instantiated.</remarks>
        protected virtual void OnStart() { }

        /// <summary>
        ///     Called when the game is ending, before <see cref="OnDisabled" />.
        ///     <para>You should not clean up the object in this method! Use <see cref="OnDisabled" /> instead.</para>
        /// </summary>
        /// <remarks>This can also be called during the game is the scriptable object is destroyed and/or unloaded.</remarks>
        protected virtual void OnPreDisabled() { }

        /// <summary>
        ///     Called when the game is ending, after <see cref="OnPreDisabled" />.
        ///     <para>
        ///         You should not unsubscribe from events in this method! Use <see cref="OnPreDisabled" /> instead.
        ///         <see cref="OnDisabled" /> should be used for cleaning up the scriptable object.
        ///     </para>
        /// </summary>
        /// <remarks>This can also be called during the game is the scriptable object is destroyed and/or unloaded.</remarks>
        protected virtual void OnDisabled() { }

        /// <inheritdoc />
        void IScriptableValueCallbacks.OnScriptableObjectPreEnable()
        {
            ResetStackTraces();
            OnPreStart();
        }

        /// <inheritdoc />
        void IScriptableValueCallbacks.OnScriptableObjectEnable()
        {
            OnStart();
        }

        /// <inheritdoc />
        void IScriptableValueCallbacks.OnScriptableObjectPreDisable()
        {
            OnPreDisabled();
        }

        /// <inheritdoc />
        void IScriptableValueCallbacks.OnScriptableObjectDisable()
        {
            OnDisabled();
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Called when the play mode state changes.
        /// </summary>
        /// <param name="state">The new play mode state.</param>
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                ResetStackTraces();
                ScriptableValuesInitialization.RegisterForEnable(this);
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                ScriptableValuesInitialization.RegisterForDisable(this);
#pragma warning disable CS0618 // Type or member is obsolete
                OnExitPlayMode();
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        /// <summary>
        ///     Called when the game exits play mode.
        /// </summary>
        [Obsolete("Use either 'OnPreDisabled' or 'OnDisabled' instead.")]
        [ExcludeFromCodeCoverage]
        protected virtual void OnExitPlayMode() { }
#endif
    }
}