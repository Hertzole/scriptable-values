#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using AuroraPunks.ScriptableValues.Debugging;
using AuroraPunks.ScriptableValues.Helpers;

namespace AuroraPunks.ScriptableValues
{
	public partial class ScriptableDictionary<TKey, TValue> : IStackTraceProvider
	{
		List<StackTraceEntry> IStackTraceProvider.Invocations { get; } = new List<StackTraceEntry>();
		event Action IStackTraceProvider.OnStackTraceAdded { add { OnStackTraceAddedInternal += value; } remove { OnStackTraceAddedInternal -= value; } }

		private event Action OnStackTraceAddedInternal;

		protected void AddStackTrace(StackTrace trace)
		{
			// Insert first so the most recent invocation is at the top.
			((IStackTraceProvider) this).Invocations.Insert(0, new StackTraceEntry(trace));

			if (((IStackTraceProvider) this).Invocations.Count > IStackTraceProvider.MAX_STACK_TRACE_ENTRIES)
			{
				((IStackTraceProvider) this).Invocations.RemoveAt(((IStackTraceProvider) this).Invocations.Count - 1);
			}

			OnStackTraceAddedInternal?.Invoke();
		}

		protected void ResetStackTraces()
		{
			((IStackTraceProvider) this).Invocations.Clear();
		}
	}
}
#endif