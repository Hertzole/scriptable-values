#nullable enable

using System.Diagnostics;
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Hertzole.ScriptableValues.Debugging;
#endif

namespace Hertzole.ScriptableValues
{
    public partial class RuntimeScriptableObject
#if UNITY_EDITOR
        : IStackTraceProvider
#endif
    {
        /// <summary>
        ///     Adds a stack trace.
        /// </summary>
        /// <remarks>This only does something in the Unity editor. This method is completely blank in builds.</remarks>
        /// <param name="skipFrames">How many frames/calls to skip.</param>
        [Conditional("UNITY_EDITOR")]
        protected void AddStackTrace(int skipFrames = 0)
        {
#if UNITY_EDITOR
            if (!((IStackTraceProvider) this).CollectStackTraces || !UserSettings.CollectStackTraces)
            {
                return;
            }

            // Always skip one frame because we don't want to include this current method.
            StackTrace trace = new StackTrace(1 + skipFrames, true);

            // Insert first so the most recent invocation is at the top.
            ((IStackTraceProvider) this).Invocations.Insert(0, new StackTraceEntry(trace));

            if (((IStackTraceProvider) this).Invocations.Count > IStackTraceProvider.MAX_STACK_TRACE_ENTRIES)
            {
                ((IStackTraceProvider) this).Invocations.RemoveAt(((IStackTraceProvider) this).Invocations.Count - 1);
            }

            OnStackTraceAddedInternal?.Invoke();
#endif
        }

        /// <summary>
        ///     Removes all stack traces.
        /// </summary>
        /// <remarks>This only does something in the Unity editor. This method is completely blank in builds.</remarks>
        [Conditional("UNITY_EDITOR")]
        protected void ResetStackTraces()
        {
#if UNITY_EDITOR
            ((IStackTraceProvider) this).Invocations.Clear();
#endif
        }
#if UNITY_EDITOR
        bool IStackTraceProvider.CollectStackTraces
        {
            get { return UserSettings.GetCollectStackTraces(this); }
            set { UserSettings.SetCollectStackTraces(this, value); }
        }

        IList<StackTraceEntry> IStackTraceProvider.Invocations { get; } = new List<StackTraceEntry>();
        event Action IStackTraceProvider.OnStackTraceAdded
        {
            add { OnStackTraceAddedInternal += value; }
            remove { OnStackTraceAddedInternal -= value; }
        }

        private event Action? OnStackTraceAddedInternal;
#endif
    }
}