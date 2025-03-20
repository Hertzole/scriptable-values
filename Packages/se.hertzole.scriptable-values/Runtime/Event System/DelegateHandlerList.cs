#nullable enable

using System;

namespace Hertzole.ScriptableValues
{
	internal sealed class DelegateHandlerList<TDelegate, T1> : BaseDelegateHandlerList<TDelegate, Action<T1, object?>> where TDelegate : Delegate
	{
		public void Invoke(T1 value)
		{
			using SpanOwner<StructClosure<Action<T1, object?>>> owner = GetCallbacks();
			Span<StructClosure<Action<T1, object?>>> span = owner.Span;
			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value, span[i].context);
			}
		}
	}

	internal sealed class DelegateHandlerList<TDelegate, T1, T2> : BaseDelegateHandlerList<TDelegate, Action<T1, T2, object?>> where TDelegate : Delegate
	{
		public void Invoke(T1 value1, T2 value2)
		{
			using SpanOwner<StructClosure<Action<T1, T2, object?>>> owner = GetCallbacks();
			Span<StructClosure<Action<T1, T2, object?>>> span = owner.Span;
			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1, value2, span[i].context);
			}
		}
	}
}