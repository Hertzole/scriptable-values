#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hertzole.ScriptableValues
{
    internal static class ScriptableValuesPlayerLoop
    {
        private static readonly List<IScriptableValueCallbacks> objectsToEnable = new List<IScriptableValueCallbacks>();
        private static readonly List<IScriptableValueCallbacks> objectsToDisable = new List<IScriptableValueCallbacks>();

        [InitializeOnLoadMethod]
        private static void InitInEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            // When exiting play mode, we want to call OnScriptableObjectPreDisable and OnScriptableObjectDisable on all registered objects.
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                for (int i = 0; i < objectsToDisable.Count; i++)
                {
                    objectsToDisable[i].OnScriptableObjectPreDisable();
                }

                for (int i = 0; i < objectsToDisable.Count; i++)
                {
                    objectsToDisable[i].OnScriptableObjectDisable();
                }

                objectsToDisable.Clear();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            // When the game starts, call OnScriptableObjectPreEnable and OnScriptableObjectEnable on all registered objects.
            for (int i = 0; i < objectsToEnable.Count; i++)
            {
                objectsToEnable[i].OnScriptableObjectPreEnable();
            }

            for (int i = 0; i < objectsToEnable.Count; i++)
            {
                objectsToEnable[i].OnScriptableObjectEnable();
            }

            objectsToEnable.Clear();
        }

        public static void RegisterForEnable(IScriptableValueCallbacks obj)
        {
            // This is a bootstrapper, we can't register for enable if the application is already playing. The object should call those methods directly.
            if (Application.isPlaying)
            {
                throw new InvalidOperationException("Cannot register for enable when the application is already playing.");
            }

            if (!objectsToEnable.Contains(obj))
            {
                objectsToEnable.Add(obj);
            }
        }

        public static void RegisterForDisable(IScriptableValueCallbacks obj)
        {
            if (!Application.isPlaying)
            {
                throw new InvalidOperationException("Cannot register for disable when the application is not playing.");
            }

            if (!objectsToDisable.Contains(obj))
            {
                objectsToDisable.Add(obj);
            }
        }
    }
}
#endif