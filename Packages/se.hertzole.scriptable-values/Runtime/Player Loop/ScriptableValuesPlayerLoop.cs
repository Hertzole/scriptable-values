using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Hertzole.ScriptableValues
{
    internal sealed class ScriptableValuesUpdate { }

    internal static class ScriptableValuesPlayerLoop
    {
        private static readonly List<IScriptableValueCallbacks> objectsToEnable = new List<IScriptableValueCallbacks>();
        private static readonly List<IScriptableValueCallbacks> objectsToDisable = new List<IScriptableValueCallbacks>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            PlayerLoopSystem defaultLoop = PlayerLoop.GetCurrentPlayerLoop();

            PlayerLoopSystem customUpdate = new PlayerLoopSystem
            {
                updateDelegate = OnUpdate,
                type = typeof(ScriptableValuesUpdate),
                subSystemList = null
            };

            PlayerLoopSystem loopWithUpdate = InsertSystemAfter<EarlyUpdate>(in defaultLoop, in customUpdate);
            PlayerLoop.SetPlayerLoop(loopWithUpdate);
        }

        public static void RegisterForEnable(IScriptableValueCallbacks obj)
        {
            if (!objectsToEnable.Contains(obj))
            {
                objectsToEnable.Add(obj);
            }
        }

        public static void RegisterForDisable(IScriptableValueCallbacks obj)
        {
            if (!objectsToDisable.Contains(obj))
            {
                objectsToDisable.Add(obj);
            }
        }

        private static void OnUpdate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif

            for (int i = 0; i < objectsToEnable.Count; i++)
            {
                objectsToEnable[i].OnScriptableObjectPreEnable();
            }

            for (int i = 0; i < objectsToEnable.Count; i++)
            {
                objectsToEnable[i].OnScriptableObjectEnable();
            }

            for (int i = 0; i < objectsToDisable.Count; i++)
            {
                objectsToDisable[i].OnScriptableObjectPreDisable();
            }

            for (int i = 0; i < objectsToDisable.Count; i++)
            {
                objectsToDisable[i].OnScriptableObjectDisable();
            }

            objectsToEnable.Clear();
            objectsToDisable.Clear();
        }

        private static PlayerLoopSystem InsertSystemAfter<T>(in PlayerLoopSystem loopSystem, in PlayerLoopSystem newSystem) where T : struct
        {
            // Create a new root PlayerLoopSystem
            PlayerLoopSystem newPlayerLoop = new PlayerLoopSystem
            {
                loopConditionFunction = loopSystem.loopConditionFunction,
                type = loopSystem.type,
                updateDelegate = loopSystem.updateDelegate,
                updateFunction = loopSystem.updateFunction
            };

            // Create a new list to populate with subsystems, including the custom system
            List<PlayerLoopSystem> newSubSystemList = new List<PlayerLoopSystem>();

            //Iterate through the subsystems in the existing loop we passed in and add them to the new list
            if (loopSystem.subSystemList != null)
            {
                for (int i = 0; i < loopSystem.subSystemList.Length; i++)
                {
                    newSubSystemList.Add(loopSystem.subSystemList[i]);
                    // If the previously added subsystem is of the type to add after, add the custom system
                    if (loopSystem.subSystemList[i].type == typeof(T))
                    {
                        newSubSystemList.Add(newSystem);
                    }
                }
            }

            newPlayerLoop.subSystemList = newSubSystemList.ToArray();
            return newPlayerLoop;
        }
    }
}