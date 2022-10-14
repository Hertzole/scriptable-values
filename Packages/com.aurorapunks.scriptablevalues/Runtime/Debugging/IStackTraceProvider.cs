#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using AuroraPunks.ScriptableValues.Helpers;

namespace AuroraPunks.ScriptableValues.Debugging
{
	public interface IStackTraceProvider
	{
		List<StackTraceEntry> Invocations { get; }
		
		event Action OnStackTraceAdded;
		
		const int MAX_STACK_TRACE_ENTRIES = 100;
	}
}
#endif