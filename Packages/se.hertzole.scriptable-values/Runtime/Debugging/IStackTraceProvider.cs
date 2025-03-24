#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace Hertzole.ScriptableValues.Debugging
{
	public interface IStackTraceProvider
	{
		bool CollectStackTraces { get; set; }

		IList<StackTraceEntry> Invocations { get; }

		event Action OnStackTraceAdded;

		const int MAX_STACK_TRACE_ENTRIES = 100;
	}
}
#endif