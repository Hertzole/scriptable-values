#nullable enable

using System;

namespace Hertzole.ScriptableValues
{
	public sealed class DelegateHandlerList<TDelegate, T1> : BaseDelegateHandlerList<TDelegate, Action<T1, object?>> where TDelegate : Delegate
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

	public sealed class DelegateHandlerList<TDelegate, T1, T2> : BaseDelegateHandlerList<TDelegate, Action<T1, T2, object?>> where TDelegate : Delegate
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

	public sealed class DelegateHandlerList<TDelegate, T1, T2, T3> : BaseDelegateHandlerList<TDelegate, Action<T1, T2, T3, object?>> where TDelegate : Delegate
	{
		public void Invoke(T1 value1, T2 value2, T3 value3)
		{
			using SpanOwner<StructClosure<Action<T1, T2, T3, object?>>> owner = GetCallbacks();
			Span<StructClosure<Action<T1, T2, T3, object?>>> span = owner.Span;
			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1, value2, value3, span[i].context);
			}
		}
	}

	public sealed class DelegateHandlerList<TDelegate, T1, T2, T3, T4> : BaseDelegateHandlerList<TDelegate, Action<T1, T2, T3, T4, object?>>
		where TDelegate : Delegate
	{
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4)
		{
			using SpanOwner<StructClosure<Action<T1, T2, T3, T4, object?>>> owner = GetCallbacks();
			Span<StructClosure<Action<T1, T2, T3, T4, object?>>> span = owner.Span;
			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1, value2, value3, value4, span[i].context);
			}
		}
	}

	public sealed class DelegateHandlerList<TDelegate, T1, T2, T3, T4, T5> : BaseDelegateHandlerList<TDelegate, Action<T1, T2, T3, T4, T5, object?>>
		where TDelegate : Delegate
	{
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
		{
			using SpanOwner<StructClosure<Action<T1, T2, T3, T4, T5, object?>>> owner = GetCallbacks();
			Span<StructClosure<Action<T1, T2, T3, T4, T5, object?>>> span = owner.Span;
			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1, value2, value3, value4, value5, span[i].context);
			}
		}
	}

	public sealed class DelegateHandlerList<TDelegate, T1, T2, T3, T4, T5, T6> : BaseDelegateHandlerList<TDelegate, Action<T1, T2, T3, T4, T5, T6, object?>>
		where TDelegate : Delegate
	{
		public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
		{
			using SpanOwner<StructClosure<Action<T1, T2, T3, T4, T5, T6, object?>>> owner = GetCallbacks();
			Span<StructClosure<Action<T1, T2, T3, T4, T5, T6, object?>>> span = owner.Span;
			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1, value2, value3, value4, value5, value6,
					span[i].context);
			}
		}
	}

	public sealed class DelegateHandlerList<TDelegate, T1, T2, T3, T4, T5, T6, T7> : BaseDelegateHandlerList<TDelegate,
		Action<T1, T2, T3, T4, T5, T6, T7, object?>>
		where TDelegate : Delegate
	{
		public void Invoke(T1 value1,
			T2 value2,
			T3 value3,
			T4 value4,
			T5 value5,
			T6 value6,
			T7 value7)
		{
			using SpanOwner<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, object?>>> owner = GetCallbacks();
			Span<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, object?>>> span = owner.Span;
			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1, value2,
					value3,
					value4,
					value5,
					value6,
					value7,
					span[i].context);
			}
		}
	}

	public sealed class DelegateHandlerList<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8> : BaseDelegateHandlerList<TDelegate,
		Action<T1, T2, T3, T4, T5, T6, T7, T8, object?>>
		where TDelegate : Delegate
	{
		public void Invoke(T1 value1,
			T2 value2,
			T3 value3,
			T4 value4,
			T5 value5,
			T6 value6,
			T7 value7,
			T8 value8)
		{
			using SpanOwner<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, object?>>> owner = GetCallbacks();

			Span<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, object?>>> span = owner.Span;

			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1,
					value2,
					value3,
					value4,
					value5,
					value6,
					value7,
					value8,
					span[i].context);
			}
		}
	}

	public sealed class DelegateHandlerList<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9> : BaseDelegateHandlerList<TDelegate,
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, object?>> where TDelegate : Delegate
	{
		public void Invoke(T1 value1,
			T2 value2,
			T3 value3,
			T4 value4,
			T5 value5,
			T6 value6,
			T7 value7,
			T8 value8,
			T9 value9)
		{
			using SpanOwner<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, object?>>> owner = GetCallbacks();

			Span<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, object?>>> span = owner.Span;

			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1,
					value2,
					value3,
					value4,
					value5,
					value6,
					value7,
					value8,
					value9,
					span[i].context);
			}
		}
	}

	public sealed class DelegateHandlerList<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : BaseDelegateHandlerList<TDelegate,
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object?>> where TDelegate : Delegate
	{
		public void Invoke(T1 value1,
			T2 value2,
			T3 value3,
			T4 value4,
			T5 value5,
			T6 value6,
			T7 value7,
			T8 value8,
			T9 value9,
			T10 value10)
		{
			using SpanOwner<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object?>>> owner = GetCallbacks();

			Span<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object?>>> span = owner.Span;

			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1,
					value2,
					value3,
					value4,
					value5,
					value6,
					value7,
					value8,
					value9,
					value10,
					span[i].context);
			}
		}
	}

	public sealed class DelegateHandlerList<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : BaseDelegateHandlerList<TDelegate,
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object?>> where TDelegate : Delegate
	{
		public void Invoke(T1 value1,
			T2 value2,
			T3 value3,
			T4 value4,
			T5 value5,
			T6 value6,
			T7 value7,
			T8 value8,
			T9 value9,
			T10 value10,
			T11 value11)
		{
			using SpanOwner<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object?>>> owner = GetCallbacks();

			Span<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object?>>> span = owner.Span;

			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1,
					value2,
					value3,
					value4,
					value5,
					value6,
					value7,
					value8,
					value9,
					value10,
					value11,
					span[i].context);
			}
		}
	}

	public sealed class DelegateHandlerList<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : BaseDelegateHandlerList<TDelegate,
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object?>> where TDelegate : Delegate
	{
		public void Invoke(T1 value1,
			T2 value2,
			T3 value3,
			T4 value4,
			T5 value5,
			T6 value6,
			T7 value7,
			T8 value8,
			T9 value9,
			T10 value10,
			T11 value11,
			T12 value12)
		{
			using SpanOwner<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object?>>> owner = GetCallbacks();

			Span<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object?>>> span = owner.Span;

			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1,
					value2,
					value3,
					value4,
					value5,
					value6,
					value7,
					value8,
					value9,
					value10,
					value11,
					value12,
					span[i].context);
			}
		}
	}

	public sealed class DelegateHandlerList<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : BaseDelegateHandlerList<TDelegate,
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object?>> where TDelegate : Delegate
	{
		public void Invoke(T1 value1,
			T2 value2,
			T3 value3,
			T4 value4,
			T5 value5,
			T6 value6,
			T7 value7,
			T8 value8,
			T9 value9,
			T10 value10,
			T11 value11,
			T12 value12,
			T13 value13)
		{
			using SpanOwner<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object?>>> owner = GetCallbacks();

			Span<StructClosure<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object?>>> span = owner.Span;

			for (int i = 0; i < owner.Length; i++)
			{
				span[i].action.Invoke(value1,
					value2,
					value3,
					value4,
					value5,
					value6,
					value7,
					value8,
					value9,
					value10,
					value11,
					value12,
					value13,
					span[i].context);
			}
		}
	}
}