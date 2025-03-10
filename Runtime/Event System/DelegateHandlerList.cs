#nullable enable

using System;

namespace Hertzole.ScriptableValues
{
	internal sealed class DelegateHandlerList<TDelegate, T1> : BaseDelegateHandlerList<TDelegate, Action<T1, object?>> where TDelegate : Delegate
	{
		public void Invoke(T1 value)
		{
			ReadOnlySpan<StructClosure<Action<T1, object?>>> span = GetCallbacks();
			for (int i = 0; i < span.Length; i++)
			{
				span[i].action.Invoke(value, span[i].context);
			}
		}
	}

	internal sealed class DelegateHandlerList<TDelegate, T1, T2> : BaseDelegateHandlerList<TDelegate, Action<T1, T2, object?>> where TDelegate : Delegate
	{
		public void Invoke(T1 value1, T2 value2)
		{
			ReadOnlySpan<StructClosure<Action<T1, T2, object?>>> span = GetCallbacks();
			for (int i = 0; i < span.Length; i++)
			{
				span[i].action.Invoke(value1, value2, span[i].context);
			}
		}
	}
}