#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace Hertzole.ScriptableValues.Debugging
{
    /// <summary>
    ///     Provides stack traces.
    /// </summary>
    /// <remarks>This type only exists in the Unity editor and will be removed in builds!</remarks>
    public interface IStackTraceProvider
    {
        /// <summary>
        ///     If <c>true</c>, stack traces will be collected.
        /// </summary>
        bool CollectStackTraces { get; set; }

        /// <summary>
        ///     A list of stack traces.
        /// </summary>
        IList<StackTraceEntry> Invocations { get; }

        /// <summary>
        ///     When a new stack trace is added.
        /// </summary>
        event Action OnStackTraceAdded;

        /// <summary>
        ///     How many stack traces to keep.
        /// </summary>
        const int MAX_STACK_TRACE_ENTRIES = 100;
    }
}
#endif